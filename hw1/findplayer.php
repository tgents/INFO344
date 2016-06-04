<?php
  try {
    $query = 'SELECT * FROM Players';
    if(isset($_GET['player'])) {
      $searchStr = $_GET['player'];
      if($searchStr != "") {
        $query .= " WHERE Name = '$searchStr'";
      }
    } else {
      $query .= " WHERE Name = 'a'";
    }

    $conn = new PDO('mysql:host=info344.cnk2klmgrohz.us-west-2.rds.amazonaws.com;port=3306;dbname=info344', 'info344user', 'info344userpassword');
    $stmt = $conn->prepare($query);
    $stmt->execute();

    $result = $stmt->fetchAll();

    header('Content-Type: application/json');
    echo $_GET['callback'] . '(' . json_encode($result) . ')';

  } catch(PDOException $e) {
    echo $_GET['callback'] . '(' . json_encode('ERROR: ' . $e->getMessage()) . ')';
  }
?>