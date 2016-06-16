<?php
	require_once(dirname(__FILE__).'/ConnectionInfo.php');
	
if (isset($_POST['id_fisc']) && isset($_POST['pass']))
{
	//Get the POST variables
	$id = $_POST['id_fisc'];
	$pass = $_POST['pass'];
	
	//Set up connection
	$connectionInfo = new ConnectionInfo();
	$connectionInfo->GetConnection();
	
	if (!$connectionInfo->conn)
	{
		//Connection failed
		echo 'No Connection';
	}
	
	else
	{
		$query = 'SELECT * FROM [Fiscal] WHERE id_fisc = ? AND pass = ? ';
		$parameters = array($id, $pass);
		$options =  array( "Scrollable" => SQLSRV_CURSOR_KEYSET );
		
		//Execute query
		$stmt = sqlsrv_query($connectionInfo->conn, $query, $parameters, $options);

		$rows = sqlsrv_num_rows($stmt);
		if ($rows > 0){
			echo "ok";
		}else{
			echo "ko";
		}
	}
}

?>