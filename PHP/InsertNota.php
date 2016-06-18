<?php
    require_once(dirname(__FILE__).'/ConnectionInfo.php');

    
if (isset($_POST['descricao']) && isset($_POST['visita']) && isset($_POST['nota']))
{
    //Get the POST variables
    $descricao = $_POST['descricao'];
    $visita = $_POST['visita'];
    $nota=$_POST['nota'];
    
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
        
        
        //Insert foto
        $query = 'INSERT INTO Nota(descricao,visita,text_file) VALUES (? , ? , ?)';
        //$parameters = array($foto, $idVis);
        $parameters = array($descricao, $visita, $nota);

        //Execute query
        $stmt = sqlsrv_query($connectionInfo->conn, $query, $parameters);
        
        if (!$stmt)
        {   //The query failed
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