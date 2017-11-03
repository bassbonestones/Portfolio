
package stones_jeremiah_extra.credit_polymorphism_javafx;

import java.util.Random;
import javafx.application.Application;
import javafx.event.ActionEvent;
import javafx.event.EventHandler;
import javafx.geometry.Pos;
import javafx.scene.Scene;
import javafx.scene.control.Button;
import javafx.scene.image.Image;
import javafx.scene.image.ImageView;
import javafx.stage.Stage;
import javafx.scene.layout.VBox;
import javafx.scene.media.AudioClip;
import javafx.scene.text.Text;
import javafx.scene.text.TextFlow;


class SpaceShip {
    
    void play() {
    }
    void changeSound(AudioClip a) {
    }
    void newImage(ImageView iv) {
    }
    
    String fire() {
        return "";
    }
}

class Drone extends SpaceShip {
    String soundName = "file:resources/pewpew.wav";
    AudioClip sound;
    
    public Drone() {
        this.sound = new AudioClip(this.soundName);
    }
    
    @Override
    void play() {
        sound.play();
    }
    Image image = new Image("file:resources/drone.png");
    @Override
    void newImage(ImageView iv) {
        iv.setImage(image);
    }
    @Override
    String fire() {
        return "pew pew!";
    }
}


class Fighter extends SpaceShip {
    String soundName = "file:resources/fighter.wav";
    AudioClip sound;
    
    public Fighter() {
        this.sound = new AudioClip(this.soundName);
    }
    
    @Override
    void play() {
        sound.play();
    }
    Image image = new Image("file:resources/fighter.png");
    @Override
    void newImage(ImageView iv) {
        iv.setImage(image);
    }
    @Override
    String fire() {
        return "The fighter fires twin photon cannons!";
    }
    
}

class Destroyer extends SpaceShip {
    String soundName = "file:resources/destroyer.wav";
    AudioClip sound;
    
    public Destroyer() {
        this.sound = new AudioClip(this.soundName);
    }
    
    @Override
    void play() {
        sound.play();
    }
    Image image = new Image("file:resources/destroyer.png");
    @Override
    void newImage(ImageView iv) {
        iv.setImage(image);
    }
    @Override
    String fire() {
        return "The Destroyer ship unleashes a blinding array of plasma bolts";
    }
}

class END extends SpaceShip {
    Image image = new Image("file:resources/endOfWar.png");
    @Override
    void newImage(ImageView iv) {
        iv.setImage(image);
    }
    @Override
    String fire() {
        return "\t      The War is Over!";
    }
}
class NEW extends SpaceShip {
    Image image = new Image("file:resources/blank.png");
    @Override
    void newImage(ImageView iv) {
        iv.setImage(image);
    }
    @Override
    String fire() {
        return "";
    }
}

public class Stones_Jeremiah_ExtraCredit_Polymorphism_JavaFX extends Application {
    int ctr = 0;
    
    @Override
    public void start(Stage primaryStage) {
        

       
        
        
        
        SpaceShip spaceShip = null;
        Drone drone = new Drone();
        Fighter fighter = new Fighter();
        Destroyer destroyer = new Destroyer();
        END end = new END();
        Random random = new Random();
        ImageView image = new ImageView("file:resources/blank.png");

        Image endOfWarImg = new Image("file:resources/endOfWar.png");
        Button btn = new Button();
        Button btn2 = new Button();
        Text fireText = new Text("");
        TextFlow txtflw = new TextFlow(fireText);
        btn.setText("Begin War!");
        btn.setOnAction(new EventHandler<ActionEvent>() {
            @Override
            public void handle(ActionEvent event) {
                SpaceShip spaceShip;
                Drone drone = new Drone();
                Fighter fighter = new Fighter();
                Destroyer destroyer = new Destroyer();
                Random random = new Random();
                int choice = random.nextInt(3);
                spaceShip = new SpaceShip();
                if (ctr == 0)
                    btn.setText("Next Ship Fires!");
                if (ctr > 19)
                    btn.setText("All Ships Destroyed!");
            switch(choice) {
                case 0:
                    spaceShip = drone;
                    ctr++;
                    break;
                case 1:
                    spaceShip = fighter;
                    ctr++;
                    break;
                case 2:
                    spaceShip = destroyer;
                    ctr++;
                    break;
            }
            String newFire = spaceShip.fire();
            
            if(ctr < 21){
                spaceShip.newImage(image);
                fireText.setText("Ship " + ctr + " fires!\n" + newFire);
                spaceShip.play();
                
            }
            else  {
                spaceShip = end;
                newFire = spaceShip.fire();
                fireText.setText(newFire);
                spaceShip.newImage(image);
            }
            }
        });
        
        btn2.setText("RESET WAR!");
        btn2.setOnAction((ActionEvent event) -> {
            ctr = 0;
            NEW new1 = new NEW();
            SpaceShip spaceShip1 = new1;
            spaceShip1.newImage(image);
            spaceShip1.fire();
            btn.setText("Begin War!");
        });
        txtflw.setPrefHeight(50.0);
        VBox root = new VBox();
        Scene scene = new Scene(root, 250, 250);
        root.setAlignment(Pos.TOP_CENTER);
        root.getChildren().addAll(btn, image, txtflw, btn2);

        primaryStage.setTitle("!!");
        primaryStage.setScene(scene);
        primaryStage.show();
    }

    /**
     * @param args the command line arguments
     */
    public static void main(String[] args) {
        launch(args);
    }
    
}

