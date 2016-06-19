<?php
	require_once(dirname(__FILE__).'/ConnectionInfo.php');

	
if (isset($_POST['IdQuestao']) && isset($_POST['IdVis']) && isset($_POST['Resposta']))
{
	//Get the POST variables
	$idQuest = $_POST['IdQuestao'];
	$idVis = $_POST['IdVis'];
	$resposta = $_POST['Resposta'];
	
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
		
		$query = "INSERT INTO VisitaQuestao(Questao,Visita,Respota) VALUES (?, ?, ?)";
		
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