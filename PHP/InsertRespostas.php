<?php
	require_once(dirname(__FILE__).'/ConnectionInfo.php');

	
if (isset($_POST['id_vis']) && isset($_POST['total'])))
{
	//Get the POST variables
	$idVis = $_POST['id_vis'];
	$total = $_POST['total'];
	
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
		for ($i = 0; $i <= $total; $i++) {
			$query = "INSERT INTO VisitaQuestao(Questao,Visita,Respota) VALUES (?, ?, ?)";
			$resposta = $_POST[$i];
			$parameters = array($i+1, $idVis, $resposta);

			//Execute query
			$stmt = sqlsrv_query($connectionInfo->conn, $query, $parameters);
		
			if (!$stmt)
			{	//The query failed
				echo 'Query Failed';	
			}
		
			else
			{
				//The query succeeded
				echo 'Successful';	
			}
		}
	}
}

?>