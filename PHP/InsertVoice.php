<?php
	require_once(dirname(__FILE__).'/ConnectionInfo2.php');

	
if (isset($_POST['Descricao']) && isset($_POST['Voz']) && isset($_POST['IdVis']))
{
	//Get the POST variables
	$desc = $_POST['Descricao'];
	$foto = $_POST['Voz'];
	$idVis = $_POST['IdVis'];
	
	//Set up our connection
	$connectionInfo = new ConnectionInfo2();
	$connectionInfo->GetConnection();
	
	if (!$connectionInfo->conn)
	{
		//Connection failed
		echo 'No Connection';
	}
	
	else
	{
		$imgData = base64_decode($foto);
		
		//Insert foto
		$query = "INSERT INTO Voz(descricao,voz_file,Visita) VALUES
					(?, CONVERT(VARBINARY(MAX), ?), ?)";
		//$parameters = array($foto, $idVis);
		$parameters = array($desc, $imgData, $idVis);

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