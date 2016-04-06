<?php
    class Student {
        public $name = "Chris Paul";

        function __construct($n) {
            $this->name = $n;
        }

        public function SubmitHomework() {
            echo "I'll submit tomorrow<br>";
        }

        public function GetName() {
            return "Name is $this->name<br>";
        }

        public static function getAllStudents() {
            $students = array();
            $students[] = new Student("LeBron James");
            $students[] = new Student("Kobe Bryant");
            $students[] = new Student("Steph Curry");
            $students[] = new Student("Albert Crosten");
            $students[] = new Student("Batman");
            return $students;
        }
    }

    $myStudent = new Student("Chris Paul");
    $myStudent->SubmitHomework();
    echo $myStudent->GetName();

    $students = Student::getAllStudents();
    for($i = 0; $i < sizeof($students); $i++){
        echo $students[$i]->GetName();
    }

?>