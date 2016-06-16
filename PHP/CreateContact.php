<?php
	require_once(dirname(__FILE__).'/ConnectionInfo.php');

	
if (isset($_POST['Username']) && isset($_POST['Password']))
{
	//Get the POST variables
	$mUsername = $_POST['Username'];
	$mPassword = $_POST['Password'];
	
	//Set up our connection
	$connectionInfo = new ConnectionInfo();
	$connectionInfo->GetConnection();
	
	if (!$connectionInfo->conn)
	{
		//Connection failed
		echo 'No Connection';
	}
	
	else
	{
		//Insert new contact into database
		$query = 'INSERT INTO [User] (Username, Password) VALUES (?, ?)';
		$parameters = array($mUsername, $mPassword);

		//Execute query
		$stmt = sqlsrv_query($connectionInfo->conn, $query, $parameters);
		
		if (!$stmt)
		{	//The query failed
			echo 'Query Failed';	
		}
		
		else
		{
			//The query succeeded, now echo back the new contact ID
			$query = "SELECT IDENT_CURRENT('User') AS NewID";
			$stmt = sqlsrv_query($connectionInfo->conn, $query);
			
			$row = sqlsrv_fetch_array($stmt,SQLSRV_FETCH_ASSOC);
						
			echo $row['NewID'];	
		}
	}
}

?>