<?php

	$mServerName = '.\SQLEXPRESS';
	$mConnectionInfo = array("Database"=>"Teste", "UID"=>"superadmin", "PWD"=>"diogo1320");
	
	$conn = sqlsrv_connect($mServerName,$mConnectionInfo);
	if($conn){
		echo 'Connection established<br />';
	}else{
		echo 'Connection failur<br />';
		die(print_r(sqlsrv_errors(),TRUE));
	}
?>