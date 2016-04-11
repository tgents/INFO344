<html>
  <head>
    <title>NBA Players</title>
    <!-- Latest compiled and minified CSS -->
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap.min.css" integrity="sha384-1q8mTJOASx8j1Au+a5WDVnPi2lkFfwwEAa8hDDdjZlpLegxhjVME1fgjWPGmkzs7" crossorigin="anonymous">
    <link rel="stylesheet" href="style.css">
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
            $query += ' WHERE Name LIKE $searchStr'
        }

        try {
            $conn = new PDO('mysql:host=info344.cnk2klmgrohz.us-west-2.rds.amazonaws.com;port=3306;dbname=info344', 'info344user', 'info344userpassword');
            $stmt = $conn->prepare('SELECT * FROM Players');
            $stmt->execute();

            $result = $stmt->fetchAll();

            echo $result;

        } catch(PDOException $e) {
            echo 'ERROR: ' . $e->getMessage();
        }
      ?>
    </div>
    <script src="https://code.jquery.com/jquery-1.12.0.min.js"></script>
    <!-- // <script src="app.js"></script> -->
  </body>
</html>