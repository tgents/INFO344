import java.io.File;
import java.io.FileNotFoundException;
import java.io.PrintWriter;
import java.util.Scanner;

public class Filter{
	public static void main(String[] args) throws FileNotFoundException{
		Scanner console = new Scanner(System.in, "UTF-8");
        Scanner input = new Scanner(new File("wikititles.txt"));
		PrintWriter writer = new PrintWriter("wikititles2.txt");

		while(input.hasNext()) {
            String line = input.nextLine();
            if(line.matches("[a-zA-z ]+")) {
                writer.println(line);
            } else {
                System.out.println("Removed " + line);
            }
        }

        writer.close();
	}
}