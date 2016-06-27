<?php
	require_once(dirname(__FILE__).'/ConnectionInfo.php');

	
if (isset($_POST['questao']) && isset($_POST['visita']) && isset($_POST['resposta']))
{
	//Get the POST variables
	$idQuest = $_POST['questao'];
	$idVis = $_POST['visita'];
	$resposta = $_POST['resposta'];
	
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
		
		$query = 'INSERT INTO visitaquestao(questao,visita,resposta) VALUES (?, ?, ?)';
		
		$parameters = array($idQuest, $idVis, $resposta);

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

?>