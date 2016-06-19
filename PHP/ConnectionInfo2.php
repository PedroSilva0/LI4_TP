<?php

class ConnectionInfo2
{
	public $mServerName;
	public $mConnectionInfo;
	public $conn;
	
	public function GetConnection()
	{
		$this->mServerName = 'admin';
		$this->mConnectionInfo = array("Database"=>"LI4");
		//$this->mConnectionInfo = array("Database"=>"Teste");
		$this->conn = sqlsrv_connect($this->mServerName,$this->mConnectionInfo);
		
		return $this->conn;
	}
}
?>