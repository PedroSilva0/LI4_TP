<?php

	$mServerName = '.\SQLEXPRESS';
	$mConnectionInfo = array("Database"=>"LI4", "UID"=>"superadmin", "PWD"=>"diogo1320");
	
	$conn = sqlsrv_connect($mServerName,$mConnectionInfo);
	if($conn){
		echo 'Connection established<br />';
	}else{
		echo 'Connection failur<br />';
		die(print_r(sqlsrv_errors(),TRUE));
	}
?>