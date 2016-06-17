<?php
	require_once(dirname(__FILE__).'/ConnectionInfo.php');
	
if (isset($_POST['Plano']))
{
	//Get the POST variables
	$plano = $_POST['Plano'];
	
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
		$query = '  SELECT ES.nome FROM estabelecimento AS ES INNER JOIN visita AS VI 
  						ON ES.id_est=VI.estabelecimento
  						INNER JOIN plano AS PL
   							ON PL.id_plano=VI.plano
 					WHERE PL.id_plano = ?';

		$parameters = array($plano);
		
		//Execute query
		$stmt = sqlsrv_query($connectionInfo->conn, $query, $parameters);
		//$stmt = sqlsrv_query($connectionInfo->conn, $query);

		if (!$stmt)
		{
			//Query failed
			echo 'Query failed';
		}
		
		else
		{
			$estabelecimentos = array(); //Create an array to hold all of the estabelecimentos
			//Query successful, begin putting each estabelecimento into an array of estabelecimentos
			
			while ($row = sqlsrv_fetch_array($stmt,SQLSRV_FETCH_ASSOC)) //While there are still estabelecimentos
			{
				//Create an associative array to hold the current plano
				//the names must match exactly the property names in the estabelecimento class in our C# code.
				$estabelecimento = array("nome" => $row['nome']);
								 
				//Add the estabelecimento to the estabelecimentos array
				array_push($estabelecimentos, $estabelecimento);
			}
			
			//Echo out the estabelecimentos array in JSON format
			echo json_encode($estabelecimentos);
		}
	}
}

?>