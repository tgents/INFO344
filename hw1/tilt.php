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
        $query = 'SELECT * FROM Players';
        if(isset($_GET['player'])) {
          $searchStr = $_GET['player'];
          if($searchStr != "") {
            $query .= " WHERE Name LIKE '%$searchStr%'";
            echo $query;
          }
        }

        try {
          $conn = new PDO('mysql:host=info344.cnk2klmgrohz.us-west-2.rds.amazonaws.com;port=3306;dbname=info344', 'info344user', 'info344userpassword');
          $stmt = $conn->prepare($query);
          $stmt->execute();

          $result = $stmt->fetchAll();

          foreach($result as $row) {
            echo "<h2>";
            print_r($row['Name']);
            echo "</h2>";

            echo "<h3>";
            print_r($row['Team']);
            echo "</h3>";

            echo "<p>";
            echo "Games Played: ";
            print_r($row['GP']);

            echo "<br>Shots per game (made/attempts): ";
            print_r($row['FG_M']);
            echo "/";
            print_r($row['FG_A']);

            echo "<br>Threes per game (made/attempts): ";
            print_r($row['3PT_M']);
            echo "/";
            print_r($row['3PT_A']);

            echo "<br>Average points per game: ";
            print_r($row['PPG']);

            echo "</p>";

            echo "<br>";
          }

        } catch(PDOException $e) {
          echo 'ERROR: ' . $e->getMessage();
        }
      ?>
    </div>
  </body>
</html>