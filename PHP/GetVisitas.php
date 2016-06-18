<?php
	require_once(dirname(__FILE__).'/ConnectionInfo.php');
	
if (isset($_POST['id_plano']))
{
	//Get the POST variables
	$plano = $_POST['id_plano'];
	
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
		$query = '  SELECT * FROM Visita AS V INNER JOIN Plano AS P 
						ON V.plano = P.id_plano
					WHERE id_plano = ?';
					
		$parameters = array($plano);
		
		//Execute query
		$stmt = sqlsrv_query($connectionInfo->conn, $query, $parameters);

		if (!$stmt)
		{
			//Query failed
			echo 'Query failed';
		}
		
		else
		{
			$visitas = array(); //Create an array to hold all of the visitas
			//Query successful, begin putting each visita into an array of visitas
			
			while ($row = sqlsrv_fetch_array($stmt,SQLSRV_FETCH_ASSOC)) //While there are still visitas
			{
				//Create an associative array to hold the current plano
				//the names must match exactly the property names in the visita class in our C# code.
				$visita = array("id_vis" => $row['id_vis'],
								"plano" => $row['plano'],
								"estabelecimento" => $row['estabelecimento'],
								"concluido" => $row['concluido']
							   );
								 
				//Add the visita to the visitas array
				array_push($visitas, $visita);
			}
			
			//Echo out the visitas array in JSON format
			echo json_encode($visitas);
		}
	}
}

?>