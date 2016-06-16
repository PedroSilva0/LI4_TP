<?php
	require_once(dirname(__FILE__).'/ConnectionInfo.php');
	
//if (isset($_POST['Fiscal']))
//{
	//$fiscal = $_POST['Fiscal'];

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
		//Create query to retrieve all planos
		//$query = 'SELECT * FROM Plano WHERE disponivel = 1 AND Fiscal = ?';
		$query = 'SELECT * FROM Plano';
		//$parameters = array($fiscal);
		//$stmt = sqlsrv_query($connectionInfo->conn, $query, $parameters);
		$stmt = sqlsrv_query($connectionInfo->conn, $query);
		
		if (!$stmt)
		{
			//Query failed
			echo 'Query failed';
		}
		
		else
		{
			$planos = array(); //Create an array to hold all of the planos
			//Query successful, begin putting each plano into an array of planos
			
			while ($row = sqlsrv_fetch_array($stmt,SQLSRV_FETCH_ASSOC)) //While there are still planos
			{
				//Create an associative array to hold the current plano
				//the names must match exactly the property names in the plano class in our C# code.
				$plano = array("id" => $row['id_plano'],
								 "disponivel" => $row['disponivel'],
								 "fiscalExecuta" => $row['Fiscal'],
								 "fiscalCria" => $row['FiscalCriador']
								 );
								 
				//Add the plano to the planos array
				array_push($planos, $plano);
			}
			
			//Echo out the planos array in JSON format
			echo json_encode($planos);
		}
	}
//}

?>