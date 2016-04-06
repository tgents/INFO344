<?php
    if(isset($_GET['n']) && $_GET['n'] >= 2) {
        $num = $_GET['n'];
        for($i = 2; $i <= $num; $i+=2) {
            echo "$i";
        }
        
    } else {
        echo "n needs to be at least 2 to print even numbers";
    }
?>