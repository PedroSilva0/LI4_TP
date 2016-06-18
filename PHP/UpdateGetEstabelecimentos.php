<?php
	require_once(dirname(__FILE__).'/ConnectionInfo.php');
	
if (isset($_POST['Plano']) && isset($_POST['Fiscal']))
{
	//Get the POST variables
	$plano = $_POST['Plano'];
	$fiscal = $_POST['Fiscal'];
	
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
		$query1 = ' UPDATE Plano
						set	Fiscal = ?,
						disponivel = 0
					where id_plano = ?';

		$query2 = '  SELECT ES.nome, VI.id_vis FROM Estabelecimento AS ES INNER JOIN Visita AS VI 
  						ON ES.id_est=VI.estabelecimento
  						INNER JOIN Plano AS PL
   							ON PL.id_plano=VI.plano
 					WHERE PL.id_plano = ? and VI.concluido=0';

 		$parameters1 = array($fiscal, $plano);
		$parameters2 = array($plano);
		
		//Execute query
		$stmt1 = sqlsrv_query($connectionInfo->conn, $query1, $parameters1);
		$stmt2 = sqlsrv_query($connectionInfo->conn, $query2, $parameters2);
		//$stmt = sqlsrv_query($connectionInfo->conn, $query);

		if (!$stmt1)
		{
			//Query1 failed
			echo 'Query1 failed';
		}else if(!$stmt2){
			//Query2 failed
			echo 'Query2 failed';
		}
		
		else
		{
			$estabelecimentos = array(); //Create an array to hold all of the estabelecimentos
			//Query successful, begin putting each estabelecimento into an array of estabelecimentos
			
			while ($row = sqlsrv_fetch_array($stmt2,SQLSRV_FETCH_ASSOC)) //While there are still estabelecimentos
			{
				//Create an associative array to hold the current plano
				//the names must match exactly the property names in the estabelecimento class in our C# code.
				$estabelecimento = array("nome" => $row['nome'],
										 "id_vis" => $row['id_vis']
										 );
								 
				//Add the estabelecimento to the estabelecimentos array
				array_push($estabelecimentos, $estabelecimento);
			}
			
			//Echo out the estabelecimentos array in JSON format
			echo json_encode($estabelecimentos);
		}
	}
}

?>