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

		$today = date("Y-m-d"); 
		$query = 'UPDATE visita 
					set concluido=1,
					dataVisita=? 
					WHERE id_vis = ?';

		$parameters = array($today,$visita);
		
		//Execute query
		$stmt1 = sqlsrv_query($connectionInfo->conn, $query, $parameters);

		if (!$stmt1)
		{
			//Query1 failed
			echo 'Query1 failed';
		}else{
			echo 'Query sucess';
		}
			
			
	}
}


?>