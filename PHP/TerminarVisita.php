<?php
	require_once(dirname(__FILE__).'/ConnectionInfo.php');
	
if (isset($_POST['id_vis']))
{
	//Get the POST variables
	$visita = $_POST['id_vis'];
	
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
		$query = 'UPDATE visita 
					set concluido=1,
					dataVisita = ? 
					WHERE id_vis = ?';
					
		$parameters = array($visita);
		
		//Execute query
		$stmt = sqlsrv_query($connectionInfo->conn, $query, $parameters);

		if (!$stmt)
		{
			//Query failed
			echo 'Query failed';
		}
		
		else
		{
			echo 'Query sucess';
		}
			
			
	}
}


?>