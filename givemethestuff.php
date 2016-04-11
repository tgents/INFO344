<?php
    try {
        $conn = new PDO('mysql:host=info344.cnk2klmgrohz.us-west-2.rds.amazonaws.com;port=3306;dbname=info344', 'info344user', 'info344userpassword');
        $stmt = $conn->prepare('SELECT * FROM Players');
        $stmt->execute();

        $result = $stmt->fetchAll();

        header('Content-Type: application/json');
        echo json_encode($result);

    } catch(PDOException $e) {
        echo 'ERROR: ' . $e->getMessage();
    }
?>