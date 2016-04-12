<html>
  <head>
    <title>NBA Players</title>
    <!-- Latest compiled and minified CSS -->
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap.min.css" integrity="sha384-1q8mTJOASx8j1Au+a5WDVnPi2lkFfwwEAa8hDDdjZlpLegxhjVME1fgjWPGmkzs7" crossorigin="anonymous">
    <link rel="stylesheet" href="style.css">
    <script src="app2.js"></script>
  </head>
  <body class="container-fluid">
    <header>
      <h1>NBA Players</h1>
      <div class="input-group">
        <input id="player" type="text" class="form-control" placeholder="Player Name | Ex: Stephen Curry">
        <span class="input-group-btn">
          <button class="btn btn-default" type="button" onclick="search();">Search</button>
        </span>
      </div>
    </header>
    <div id="players">
      <?php
        include("player.php");

        $query = 'SELECT * FROM Players';
        if(isset($_GET['player'])) {
          $searchStr = $_GET['player'];
          if($searchStr != "") {
            $query .= " WHERE Name LIKE '%$searchStr%'";
            echo $query;
          }
        }

        try {
          $conn = new PDO('mysql:host=info344.cnk2klmgrohz.us-west-2.rds.amazonaws.com;port=3306;
            dbname=info344', 'info344user', 'info344userpassword');
          $stmt = $conn->prepare($query);
          $stmt->execute();

          $result = $stmt->fetchAll();

          foreach($result as $row) {
            $player = new Player($row['Name'], $row['Team'], $row['GP'], $row['FG_M'], 
              $row['FG_A'], $row['3PT_M'], $row['3PT_A'], $row['FT_M'], $row['FT_A'], 
              $row['Rebounds_Tot'], $row['Ast'], $row['Stl'], $row['Blk'], $row['PPG']);

            $player->printPlayer();

            echo "<br>";
          }

        } catch(PDOException $e) {
          echo 'ERROR: ' . $e->getMessage();
        }
      ?>
    </div>
  </body>
</html>