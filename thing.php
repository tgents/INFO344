<?php
    // echo 'hello world';
    try {
        $conn = new PDO('mysql:host=uwinfo344.chunkaiw.com;port=3306;dbname=info344mysqlpdo', 'info344mysqlpdo', 'chrispaul');
        $stmt = $conn->prepare('SELECT * FROM Books');
        $stmt->execute();

        $result = $stmt->fetchAll();

        foreach($result as $row) {
            print_r($row);
            echo "<br>";
        }

    } catch(PDOException $e) {
        echo 'ERROR: ' . $e->getMessage();
    }
?>