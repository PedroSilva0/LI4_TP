<?php

class ConnectionInfo2
{
	public $mServerName;
	public $mConnectionInfo;
	public $conn;
	
	public function GetConnection()
	{
		$this->mServerName = '.\SQLEXPRESS';
		$this->mConnectionInfo = array("Database"=>"LI4", "UID"=>"superadmin", "PWD"=>"diogo1320");
		//$this->mConnectionInfo = array("Database"=>"Teste");
		$this->conn = sqlsrv_connect($this->mServerName,$this->mConnectionInfo);
		
		return $this->conn;
	}
}
?>