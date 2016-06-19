<?php
	require_once(dirname(__FILE__).'/ConnectionInfo.php');

	$connectionInfo = new ConnectionInfo();
	$connectionInfo->GetConnection();
	
	if (!$connectionInfo->conn)
	{
		//Connection failed
		echo 'No Connection';
	}
	else
	{
		$query = 'SELECT * FROM Questao';

		//Execute query
		$stmt = sqlsrv_query($connectionInfo->conn, $query);
		
		if (!$stmt)
		{	//The query failed
			echo 'Query Failed';	
		}
		else
		{
			$questoes = array(); //Create an array to hold all of the questoes
			//Query successful, begin putting each questao into an array of questoes
			
			while ($row = sqlsrv_fetch_array($stmt,SQLSRV_FETCH_ASSOC)) //While there are still questoes
			{
				//Create an associative array to hold the current questao
				//the names must match exactly the property names in the questao class in our C# code.
				$questao = array("id_pergunta" => $row['id_quest'],
								 "pergunta" => $row['pergunta']
								 );
								 
				//Add the questao to the questoes array
				array_push($questoes, $questao);
			}
			
			//Echo out the questoes array in JSON format
			echo json_encode($questoes);
		}
	}
?>