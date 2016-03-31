<?
class DB {
    private $servername = "localhost";
    private $username = "gamedb";
    private $password = "gamedb";
    private $dbname = "gamedb";
    private $queryResult = false;
    public $conn = null;
    private $lastQuery = null;

    public function __construct() {
    // Create connection
        $this->conn = mysqli_connect($this->servername, $this->username, $this->password, $this->dbname);
        // Check connection
        if (!$this->conn) {
            die("Database Connection failed: " . mysqli_connect_error());
        }
        $this->conn->set_charset("utf8");
    }

    public function __destruct() {
        mysqli_close($this->conn);
    }

    public function results() {
        return mysqli_num_rows($this->queryResult);
    }

    public function fetch() {
        if($this->queryResult)
            return mysqli_fetch_assoc($this->queryResult);
        return false;
    }

    public function query() {
        $query = "";
        $parameterNumber = 0;
        $offset = 0;

        if (func_num_args() && $query = func_get_arg($parameterNumber++)) {
            while ($parameterNumber < func_num_args()) {
                $nextParameter = func_get_arg($parameterNumber++);
                $placeToInsertParameter = strpos($query, '?', $offset);
                if ($placeToInsertParameter !== false) {
                    $querySafeString = '';

                    if (is_bool($nextParameter)) {
                        $querySafeString = $nextParameter ? 'TRUE' : 'FALSE';
                    }
                    else if (is_float($nextParameter) || is_int($nextParameter)) {
                        $querySafeString = $nextParameter;
                    }
                    else if (is_null($nextParameter) || $nextParameter == "NULL") {
                        $querySafeString = 'NULL';
                    }
                    else {
                        $querySafeString = "'" . mysql_escape_string($nextParameter) . "'";
                    }

                    $offset = $placeToInsertParameter + strlen($querySafeString);
                    $query = substr_replace($query, $querySafeString, $placeToInsertParameter, 1);
                }
            }
        }

        $this->lastQuery = $query;

        if($this->queryResult = mysqli_query($this->conn, $query))
            return (strpos($query, "INSERT") === false ? true : mysqli_insert_id($this->conn));
        return false;
    }

    function getLastQuery() {
        return $this->lastQuery;
    }
}
?>