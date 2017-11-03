/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package stones_jeremiah_lab4;

import java.text.DecimalFormat;
import java.util.Optional;
import javafx.application.Application;
import static javafx.application.Application.launch;
import javafx.beans.value.ChangeListener;
import javafx.beans.value.ObservableValue;
import javafx.event.ActionEvent;
import javafx.event.EventHandler;
import javafx.geometry.Insets;
import javafx.geometry.Pos;
import javafx.scene.Scene;
import javafx.scene.control.Alert;
import javafx.scene.control.Alert.AlertType;
import javafx.scene.control.Button;
import javafx.scene.control.ButtonType;
import javafx.scene.control.ChoiceBox;
import javafx.scene.control.Label;
import javafx.scene.control.Tooltip;
import javafx.scene.image.Image;
import javafx.scene.image.ImageView;
import javafx.scene.input.MouseEvent;
import javafx.scene.layout.Background;
import javafx.scene.layout.BackgroundFill;
import javafx.scene.layout.BackgroundImage;
import javafx.scene.layout.BackgroundPosition;
import javafx.scene.layout.BackgroundRepeat;
import javafx.scene.layout.BackgroundSize;
import javafx.scene.layout.BorderPane;
import javafx.scene.layout.CornerRadii;
import javafx.scene.layout.HBox;
import javafx.scene.layout.StackPane;
import javafx.scene.layout.VBox;
import javafx.scene.paint.Color;
import javafx.scene.text.Font;
import javafx.scene.text.Text;
import javafx.scene.text.TextAlignment;
import javafx.scene.text.TextFlow;
import javafx.stage.Stage;

/**
 *
 * @author Inspiron 1525
 */

public class Stones_Jeremiah_Lab4 extends Application {
    Label sub1, sub2, sub3, sub4, sub5, total;
    Tooltip tt1, tt2, tt3, tt4, tt5;
    double thistotal;
    DecimalFormat formatter;
    Book b1, b2, b3, b4, b5;
    ImageView imagePreview, dead, ars, mozi, histories, china;
    BackgroundImage fill = new BackgroundImage(new Image("file:back.png"), BackgroundRepeat.NO_REPEAT, BackgroundRepeat.NO_REPEAT,
        BackgroundPosition.CENTER, BackgroundSize.DEFAULT);
    

    @Override
    public void start(Stage primaryStage) {
        b1 = new Book("Egyptian Book of the Dead", "A collection of spells which enable the soul of the \ndeceased to navigate the afterlife.", 81.99);
        b2 = new Book("Ars Almadel", "A significant grimoire of demonology compiled in the 17th century", 22.99);
        b3 = new Book("Mozi", "Moral teachings that emphasized self-\nreflection and authenticity rather than obedience to ritual.", 33.99);
        b4 = new Book("Herodotus' Histories", "The founding work of history in Western Literature.", 43.99);
        b5 = new Book("Book of Documents", "A collection of rhetorical prose attributed to figures of ancient China", 13.99);
        
        formatter = new DecimalFormat("##0.00");
        int x, y;
        x = 1000;
        y = 600;
        Text txt1 = new Text("Welcome to Ye Old Bookstore!\n");
        Text txt2 = new Text("Select Items to Purchase");
        txt1.setFont(Font.loadFont("file:isa_font.ttf", 35));
        txt1.setFill(Color.PURPLE);
        
        TextFlow tflow1 = new TextFlow(txt1, txt2);
        
        Button exitBtn = new Button("EXIT");
        StackPane exit = new StackPane(exitBtn);
        exitBtn.setOnAction(e -> System.exit(0));
        exit.setPadding(new Insets(0,0,0,300));
        HBox tophbox = new HBox(tflow1, exit);
        tophbox.setPadding(new Insets(10));
        tophbox.setPrefSize(x,y/5);
        
        imagePreview = new ImageView(new Image("file:Preview.png"));
        dead = new ImageView(new Image("file:dead.png"));
        mozi = new ImageView(new Image("file:Mozi.png"));
        histories = new ImageView(new Image("file:histories.png"));
        china = new ImageView(new Image("file:china.png"));
        ars = new ImageView(new Image("file:ars.png"));
        
        VBox leftvbox = new VBox(imagePreview);
        leftvbox.setPadding(new Insets(15));
        leftvbox.setPrefSize(x/5, y*3/5);

        Text descr1 = new Text(b1.name + " ($" + b1.price + ") ");
        Text descr2 = new Text(b2.name + " ($" + b2.price + ") ");
        Text descr3 = new Text(b3.name + " ($" + b3.price + ") ");
        Text descr4 = new Text(b4.name + " ($" + b4.price + ") ");
        Text descr5 = new Text(b5.name + " ($" + b5.price + ") ");
        tt1 = new Tooltip("A Cairo Bestseller!");
        tt2 = new Tooltip("One heck of a book!");
        tt3 = new Tooltip("Buy it, because it's the right thing to do!");
        tt4 = new Tooltip("April showers bring May flowers. Mayflowers bring smallpox.");
        tt5 = new Tooltip("I love China!");
        Text instr = new Text("Description: ");
        instr.setFill(Color.RED);
        Text instr2 = new Text("HOVER OVER BOOK TITLE TO VIEW");
        
        TextFlow TFdescr1 = new TextFlow(descr1);
        TFdescr1.setPrefSize(100,22);
        TFdescr1.setTextAlignment(TextAlignment.RIGHT);
        TFdescr1.setOnMouseEntered(new EventHandler<MouseEvent>() {
    
            public void handle(MouseEvent e) {
                imagePreview.setImage(dead.getImage());
                instr2.setText(b1.description);
                }
        });
        Tooltip.install(TFdescr1, tt1);
        TextFlow TFdescr2 = new TextFlow(descr2);
        TFdescr2.setPrefSize(100,22);
        TFdescr2.setTextAlignment(TextAlignment.RIGHT);
        TFdescr2.setOnMouseEntered(new EventHandler<MouseEvent>() {
    
            public void handle(MouseEvent e) {
                imagePreview.setImage(ars.getImage());
                instr2.setText(b2.description);
                }
        });
        Tooltip.install(TFdescr2, tt2);
        TextFlow TFdescr3 = new TextFlow(descr3);
        TFdescr3.setPrefSize(100,22);
        TFdescr3.setTextAlignment(TextAlignment.RIGHT);
        TFdescr3.setOnMouseEntered(new EventHandler<MouseEvent>() {
    
            public void handle(MouseEvent e) {
                imagePreview.setImage(mozi.getImage());
                instr2.setText(b3.description);
                }
        });
        Tooltip.install(TFdescr3, tt3);
        TextFlow TFdescr4 = new TextFlow(descr4);
        TFdescr4.setPrefSize(100,22);
        TFdescr4.setTextAlignment(TextAlignment.RIGHT);
        TFdescr4.setOnMouseEntered(new EventHandler<MouseEvent>() {
    
            public void handle(MouseEvent e) {
                imagePreview.setImage(histories.getImage());
                instr2.setText(b4.description);
                }
        });
        Tooltip.install(TFdescr4, tt4);
        TextFlow TFdescr5 = new TextFlow(descr5);
        TFdescr5.setPrefSize(100,22);
        TFdescr5.setTextAlignment(TextAlignment.RIGHT);
        TFdescr5.setOnMouseEntered(new EventHandler<MouseEvent>() {
    
            public void handle(MouseEvent e) {
                imagePreview.setImage(china.getImage());
                instr2.setText(b5.description);
                }
        });
        Tooltip.install(TFdescr5, tt5);
        
        VBox centervbox = new VBox(33, TFdescr1, TFdescr2,
            TFdescr3, TFdescr4, TFdescr5);
        centervbox.setPadding(new Insets(3,20,20,20));
        centervbox.setPrefSize(x*3/5, y*3/5);
        

              
        ChoiceBox choice1 = new ChoiceBox();
        choice1.getItems().addAll(0,1,2,3,4,5);
        choice1.setValue(0);
        choice1.setMaxSize(50, 22);
        ChoiceBox choice2 = new ChoiceBox();
        choice2.getItems().addAll(0,1,2,3,4,5);
        choice2.setValue(0);
        choice2.setMaxSize(50, 22);
        ChoiceBox choice3 = new ChoiceBox();
        choice3.getItems().addAll(0,1,2,3,4,5);
        choice3.setValue(0);
        choice3.setMaxSize(50, 22);
        ChoiceBox choice4 = new ChoiceBox();
        choice4.getItems().addAll(0,1,2,3,4,5);
        choice4.setValue(0);
        choice4.setMaxSize(50, 22);
        ChoiceBox choice5 = new ChoiceBox();
        choice5.getItems().addAll(0,1,2,3,4,5);
        choice5.setValue(0);
        choice5.setMaxSize(50, 22);
        Text Tot = new Text("Total:");
        TextFlow tfTot = new TextFlow(Tot);
        tfTot.setPadding(new Insets(3));
        VBox rightvbox = new VBox(30, choice1, choice2, choice3, choice4, choice5, tfTot);
        rightvbox.setPrefSize(x/10,y*3/5);
        
        sub1 = new Label("0.00");
        sub2 = new Label("0.00");
        sub3 = new Label("0.00");
        sub4 = new Label("0.00");
        sub5 = new Label("0.00");
        total = new Label("0.00");
        VBox rightvbox2 = new VBox(38, sub1, sub2, sub3, sub4, sub5, total);
        rightvbox2.setPadding(new Insets(3));

        rightvbox2.setPrefSize(x/10,y*3/5);
        
        HBox rightcenthbox = new HBox(rightvbox, rightvbox2);
        
        choice1.getSelectionModel().selectedIndexProperty().addListener(new
               ChangeListener<Number>() {
                   public void changed(ObservableValue ov,
                           Number value, Number new_value) {
                           
                            sub1.setText(String.valueOf(formatter.format((Double.valueOf(
                                    String.valueOf(new_value))*(b1.price)))));
                            set();
                   }
               });
        
        choice2.getSelectionModel().selectedIndexProperty().addListener(new
               ChangeListener<Number>() {
                   public void changed(ObservableValue ov,
                           Number value, Number new_value) {
 
                            sub2.setText(String.valueOf(formatter.format((Float.valueOf(
                                    String.valueOf(new_value))*28.99))));
                            set();
                   }
               });
        choice3.getSelectionModel().selectedIndexProperty().addListener(new
               ChangeListener<Number>() {
                   public void changed(ObservableValue ov,
                           Number value, Number new_value) {
 
                            sub3.setText(String.valueOf(formatter.format((Float.valueOf(
                                    String.valueOf(new_value))*28.99))));
                            set();
                   }
               });
        choice4.getSelectionModel().selectedIndexProperty().addListener(new
               ChangeListener<Number>() {
                   public void changed(ObservableValue ov,
                           Number value, Number new_value) {
 
                            sub4.setText(String.valueOf(formatter.format((Float.valueOf(
                                    String.valueOf(new_value))*28.99))));
                            set();
                   }
               });
        choice5.getSelectionModel().selectedIndexProperty().addListener(new
               ChangeListener<Number>() {
                   @Override
                   public void changed(ObservableValue ov,
                           Number value, Number new_value) {
 
                            sub5.setText(String.valueOf(formatter.format((Float.valueOf(
                                    String.valueOf(new_value))*28.99))));
                            set();
                   }
               });
        Button checkOut = new Button("Check Out");
        checkOut.setOnAction((ActionEvent e) -> {
            if(total.getText().equals("0.00")){
                Alert alertX = new Alert(AlertType.INFORMATION);
                alertX.setTitle("EMPTY CART MESSAGE");
                alertX.setHeaderText("Your Cart Is Empty");
                alertX.setContentText("Please continue shopping.");
                alertX.showAndWait();
                return;
            }            
            Alert alert = new Alert(AlertType.CONFIRMATION);
            alert.setTitle("Confirmation on Check Out");
            alert.setHeaderText("Would you like to confirm your purchase?");
            alert.setContentText("If you select 'OK', you will be charged $"
            + total.getText());

            Optional<ButtonType> result = alert.showAndWait();
            if (result.get() == ButtonType.OK){
                Alert alert2 = new Alert(AlertType.INFORMATION);
                alert2.setTitle("Check Out Screen");
                alert2.setHeaderText(null);
                alert2.setContentText("You have been charged $" +
                    total.getText());

                alert2.showAndWait();
                choice1.valueProperty().set(0);
                choice2.valueProperty().set(0);
                choice3.valueProperty().set(0);
                choice4.valueProperty().set(0);
                choice5.valueProperty().set(0);
            } 
            else {
                Alert alert2 = new Alert(AlertType.INFORMATION);
                alert2.setTitle("Check Out Screen");
                alert2.setHeaderText(null);
                alert2.setContentText("You have been not been charged.\n" +
                        "Please continue shopping!");

                alert2.showAndWait();    
                
            }
            
        });
        
        TextFlow instructions = new TextFlow(instr, instr2);
        instructions.setPadding(new Insets(25));
        checkOut.setFont(new Font(20));
        checkOut.setPrefSize(200, 40);
        HBox extra2 = new HBox(instructions);
        HBox extra1 = new HBox(checkOut);
        extra1.setPrefHeight(200);
        extra2.setAlignment(Pos.CENTER_LEFT);
        extra2.setPrefHeight(200);
        HBox bottomhbox = new HBox(40, extra2, extra1);
        bottomhbox.setAlignment(Pos.CENTER_RIGHT);
        bottomhbox.setPadding(new Insets(30));
        
       
        
        
        BorderPane rootNode = new BorderPane();
        rootNode.setBackground(new Background(fill));
        Scene scene = new Scene(rootNode, x, y);
        
        rootNode.setTop(tophbox);
        rootNode.setLeft(leftvbox);
        rootNode.setCenter(centervbox);
        rootNode.setRight(rightcenthbox);
        rootNode.setBottom(bottomhbox);
        primaryStage.setTitle("Jeremy's Bookstore");
        primaryStage.setScene(scene);
        primaryStage.show();
    }

    public void set() {
        double ttl;
        ttl = Double.parseDouble(String.valueOf(sub1.getText())) 
                + Double.parseDouble(String.valueOf(sub2.getText()))
                + Double.parseDouble(String.valueOf(sub3.getText()))
                + Double.parseDouble(String.valueOf(sub4.getText()))
                + Double.parseDouble(String.valueOf(sub5.getText()));
                            total.setText(String.valueOf(formatter.format(ttl)));
    }
    public static void main(String[] args) {
        launch(args);
    }
    
}
