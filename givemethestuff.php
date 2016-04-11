<?php
    if(isset($_GET['player'])) {
        $player = $_GET['player'];

        try {
        	$server = "info344.cnk2klmgrohz.us-west-2.rds.amazonaws.com";
        	$port = "3306";
        	$db = 'info344';
        	$username = 'info344user'
        	$pw = 'info344userpassword'
	        $conn = new PDO('mysql:host=$server;port=$port;dbname=$db', $username, $pw);
	        $stmt = $conn->prepare('SELECT * FROM 'TABLE 1'');
	        $stmt->execute();

	        $result = $stmt->fetchAll();

	        foreach($result as $row) {
	            print_r($row);
	            echo "<br>";
        }

	    } catch(PDOException $e) {
	        echo 'ERROR: ' . $e->getMessage();
	    }
        
    } else {
        echo "you didn't give me a player";
    }
?>