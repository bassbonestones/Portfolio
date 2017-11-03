
package gateley_stones_groupproject_client;

import com.sun.javafx.application.LauncherImpl;
import java.io.BufferedReader;
import java.io.ByteArrayInputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.PrintWriter;
import java.io.UnsupportedEncodingException;
import static java.lang.Thread.sleep;
import java.net.Socket;
import java.rmi.UnknownHostException;
import java.text.DateFormat;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;
import java.util.TimeZone;
import java.util.logging.Level;
import java.util.logging.Logger;
import javafx.application.Application;
import javafx.application.Platform;
import javafx.application.Preloader;
import javafx.beans.binding.Bindings;
import javafx.beans.property.DoubleProperty;
import javafx.beans.property.SimpleDoubleProperty;
import javafx.beans.value.ChangeListener;
import javafx.beans.value.ObservableValue;
import javafx.collections.FXCollections;
import javafx.collections.ListChangeListener;
import javafx.collections.ObservableList;
import javafx.concurrent.Task;
import javafx.event.ActionEvent;
import javafx.event.EventHandler;
import javafx.event.EventType;
import javafx.geometry.Insets;
import javafx.geometry.Pos;
import javafx.scene.Node;
import javafx.scene.Scene;
import javafx.scene.control.Button;
import javafx.scene.control.ComboBox;
import javafx.scene.control.ContentDisplay;
import javafx.scene.control.Label;
import javafx.scene.control.ListCell;
import javafx.scene.control.Menu;
import javafx.scene.control.MenuBar;
import javafx.scene.control.MenuItem;
import javafx.scene.control.ScrollPane;
import javafx.scene.control.TextArea;
import javafx.scene.control.TextFormatter.Change;
import javafx.scene.control.TreeCell;
import javafx.scene.control.TreeItem;
import static javafx.scene.control.TreeItem.childrenModificationEvent;
import javafx.scene.control.TreeView;
import javafx.scene.image.Image;
import javafx.scene.image.ImageView;
import javafx.scene.input.KeyCode;
import static javafx.scene.input.KeyCode.T;
import javafx.scene.input.KeyEvent;
import javafx.scene.input.MouseButton;
import javafx.scene.input.MouseEvent;
import javafx.scene.layout.Background;
import javafx.scene.layout.BackgroundFill;
import javafx.scene.layout.CornerRadii;
import javafx.scene.layout.FlowPane;
import javafx.scene.layout.HBox;
import javafx.scene.layout.VBox;
import javafx.scene.paint.Color;
import javafx.scene.paint.ImagePattern;
import javafx.scene.shape.Ellipse;
import javafx.scene.text.Font;
import javafx.scene.text.FontWeight;
import javafx.scene.text.TextAlignment;
import javafx.stage.Stage;

public class Gateley_Stones_GroupProject_Client extends Application {

    String username, ip, url;
    int port, imgSelection;
    boolean loggedIn;
    Date date;
    ImageView arrow;
    TreeView<String> userList = new TreeView();
    TreeItem<String> root = new TreeItem("dude");
    ScrollPane dude;
    DateFormat df = new SimpleDateFormat("h:mm a");

    Login j;
    About a;

    // counter to create some delay while showing preloader.
    private static final int COUNT_LIMIT = 100000;

    public static void main(String[] args) {
        LauncherImpl.launchApplication(Gateley_Stones_GroupProject_Client.class, MyPreloader.class, args);
    }

    @Override
    public void init() throws Exception {
        // causes a delay for the preloader, could be actual tasks in a bigger program, but not here
        for (int i = 0; i < COUNT_LIMIT; i++) {
            double progress = (100 * i) / COUNT_LIMIT;
            LauncherImpl.notifyPreloader(this, new Preloader.ProgressNotification(progress));
        }
    }
    public void addUserToGUI(String username, boolean usingPicturePreset, int imgSelection, String url) {
        TreeItem<String> newItem = new TreeItem<>();
        newItem.setValue(username);
        Ellipse e = new Ellipse(10,10);
        Image pic;
        
        if (usingPicturePreset) {
            pic = new Image(getUserPic(imgSelection));
        } else {
            pic = new Image(url);
        }
        
        e.setFill(new ImagePattern(pic));
        newItem.setGraphic(e);
        userList.getRoot().getChildren().add(newItem);
        //userList.requestFocus();
        //userList.getSelectionModel().select(newItem);
        userList.edit(newItem);
    }
    
    

    

    @Override
    public void start(Stage applicationStage) throws Exception {
        url = "empty";
        loggedIn = false;
        
        userList.setRoot(root);
        userList.setShowRoot(false);
        userList.getRoot().setExpanded(true);
        userList.setMinHeight(498);
        
        HBox chatAndUsersHB = new HBox(); // entire top portion of GUI
        chatAndUsersHB.setPrefSize(700, 500);
        chatAndUsersHB.setBackground(new Background(new BackgroundFill(Color.BLACK, CornerRadii.EMPTY, Insets.EMPTY)));
        VBox innerChatVB = new VBox();
        innerChatVB.setMinWidth(480);
        innerChatVB.setMaxWidth(480);
        innerChatVB.setMinHeight(500);
        innerChatVB.setAlignment(Pos.BOTTOM_CENTER);
        innerChatVB.setStyle("-fx-background-color: #555555");
        dude = new ScrollPane(innerChatVB);
        dude.setMaxSize(480, 500);
        dude.setMinSize(480, 500);
        dude.setHbarPolicy(ScrollPane.ScrollBarPolicy.NEVER);
        dude.setVbarPolicy(ScrollPane.ScrollBarPolicy.ALWAYS);
        dude.setStyle("-fx-background-color: #0033EE");
        HBox chatHB = new HBox(dude);
        chatHB.setStyle("-fx-background-color: #FF0000");
        chatHB.setAlignment(Pos.CENTER);
        chatHB.setPrefSize(480, 490);
        VBox usersVB = new VBox();
        usersVB.setMinSize(190, 480);
        usersVB.setMaxWidth(190);
        ScrollPane usersSP = new ScrollPane(usersVB);
        usersSP.setHbarPolicy(ScrollPane.ScrollBarPolicy.NEVER);
        usersSP.setVbarPolicy(ScrollPane.ScrollBarPolicy.AS_NEEDED);
        usersSP.setStyle("-fx-background: #555555; -fx-background-color: #99FF00");
        usersSP.setMinSize(190, 500);
        usersSP.setMaxSize(190, 500);
        chatAndUsersHB.setSpacing(10);
        chatAndUsersHB.setPadding(new Insets(7, 0, 0, 10));
        chatAndUsersHB.getChildren().addAll(chatHB, usersSP);
        VBox textInputAreaVB = new VBox();
        textInputAreaVB.setStyle("-fx-background-color: #555555");
        textInputAreaVB.setPrefSize(480, 80);
        TextArea ta = new TextArea();
        ta.setPrefSize(480, 60);
        ta.setMinSize(480, 60);
        ta.setMaxSize(480, 60);
        ta.setWrapText(true);
        String emojiNames[] = {"angry", "awwyeah", "brokenheart", "clapping", "dislike",
            "facepalm", "happy", "headbang", "heart", "hi", "idea", "kiss", "laugh", "like", "poop",
            "punch", "rock", "sad", "wow", "wtf"};
        ComboBox<String> comboBox = new ComboBox<>();
        comboBox.getItems().addAll(emojiNames);
        //Set the cellFactory property
        comboBox.setCellFactory(listview -> new StringImageCell());
        // Set the buttonCell property
        comboBox.setButtonCell(new StringImageCell());

        comboBox.setStyle("-fx-accent: #FFFFFF; -fx-focus-color: #FFFFFF; -fx-cell-hover-color: #FFFFFF");
        comboBox.setMaxSize(90, 50);
        comboBox.setPromptText("Emoji");
        comboBox.setOnAction(new EventHandler<ActionEvent>() {
            @Override
            public void handle(ActionEvent a) {
                innerChatVB.getChildren().add(sendEmoji(comboBox.getSelectionModel().getSelectedItem()));
                Platform.runLater(() -> {
                    comboBox.getSelectionModel().clearSelection();
                    getDude().setVvalue(innerChatVB.getHeight());
                        });
            }
        });
        /////////////////////////////////////////
        ////////////testing stuff/////////////
        ////////////////////////////////////////
        innerChatVB.getChildren().add(makeReceivedEmoji("wow", "otheruser", true, 3,"url"));
        addUserToGUI("dude", true, 2, "url");
        addUserToGUI("dude2", true, 5, "url");
        addUserToGUI("dude3", true, 1, "url");
        innerChatVB.getChildren().add(makeReceivedMessage("sample private from other user", "otheruser", true, 1, "web", true));
        innerChatVB.getChildren().add(makeReceivedMessage("sample normal from other user", "otheruser",true, 2, "web", false));
        innerChatVB.getChildren().add(makeLocalMessage("sample private message from this user to otheruser - enter message for normal from this user", username,true, 3, "web", true));
        usersVB.getChildren().add(userList);

        ///////////////////////////////////////
        ///////////////////////////////////////
        
        dude.setVvalue(1.0);
        
        Button sendBtn = new Button("Send");
        
        sendBtn.setOnAction(new EventHandler<ActionEvent>() {
            public void handle(ActionEvent a) {

                String message = ta.getText();
                String data = "{ \"message\": \"" + message + "\", \"ip\": \"" + ip + "\", \"imgSelection\": \"" + imgSelection
                        + "\", \"url\": \"" + url + "\", \"port\": \"" + port + "\", \"username\": \"" + username + "\" }";
                System.out.println(data);
                if (message.equals("")) {
                    return;
                }
                boolean wasOther = false;
                if (message.length() > 5) {
                    String toTTS = message.substring(0, 4).toLowerCase();
                    //System.out.println(toTTS);
                    if (toTTS.equals("\\tts")) {
                        //change text to speech
                        wasOther = true;

                    }
                }
                for (int i = 0; i < 20; i++) {
                    if (message.equals(emojiNames[i])) {
                        wasOther = true;
                        innerChatVB.getChildren().add(sendEmoji(message));
                        Platform.runLater(() -> dude.setVvalue(innerChatVB.getHeight()));
                    }
                }

                if (wasOther == false) {

                    try {
                        InputStream testInput = new ByteArrayInputStream(ta.getText().getBytes("UTF-8"));
                        System.setIn(testInput);

                    } catch (UnsupportedEncodingException ex) {
                        Logger.getLogger(Gateley_Stones_GroupProject_Client.class.getName()).log(Level.SEVERE, null, ex);
                        System.out.println("didn't set inputstream");
                    }
                    innerChatVB.getChildren().add(makeLocalMessage(ta.getText(), username, true, imgSelection, url, false));
                    Platform.runLater(() -> {
                        try {
                            sleep(500);
                            Platform.runLater(() -> dude.setVvalue(innerChatVB.getHeight()));
                        } catch (InterruptedException ex) {
                            Logger.getLogger(Gateley_Stones_GroupProject_Client.class.getName()).log(Level.SEVERE, null, ex);
                        }
                    });
                }
                ta.clear();
                ta.requestFocus();
            }
        });
        sendBtn.setOnKeyPressed(new EventHandler<KeyEvent>() {
            @Override
            public void handle(KeyEvent k) {
                if (k.getCode() == KeyCode.ENTER) {
                    sendBtn.fire();
                    // clear text
                    k.consume();
                }
            }
        });
        ta.setOnKeyPressed(new EventHandler<KeyEvent>() {
            @Override
            public void handle(KeyEvent k) {
                if (k.getCode() == KeyCode.ENTER) {
                    sendBtn.fire();
                    // clear text
                    k.consume();
                }
                if (k.getCode() == KeyCode.TAB) {
                    k.consume();
                    sendBtn.requestFocus();
                }
            }
        });
        
        
        
        FlowPane fpExtras = new FlowPane(sendBtn, comboBox);
        fpExtras.setStyle("-fx-background-color: #555555");
        fpExtras.setMaxSize(200, 80); 
        textInputAreaVB.getChildren().addAll(ta);
        
        HBox inputAreaHB = new HBox(textInputAreaVB, fpExtras);
        inputAreaHB.setPadding(new Insets(0, 10, 10, 10));
        inputAreaHB.setPrefHeight(70);
        
        MenuBar menuBar = new MenuBar();
        Menu file = new Menu("File");
        MenuItem signIn = new MenuItem("Sign In");

        DoubleProperty wProperty = new SimpleDoubleProperty();
        wProperty.bind(innerChatVB.heightProperty()); // bind to Hbox width chnages
        wProperty.addListener(new ChangeListener() {
            @Override
            public void changed(ObservableValue ov, Object t, Object t1) {
                //when ever Hbox width chnages set ScrollPane Hvalue
                dude.setVvalue(innerChatVB.getHeight());
            }
        });

        signIn.setOnAction(new EventHandler<ActionEvent>() {
            public void handle(ActionEvent t) {
                if (port > 0) {
                    return;
                }
                j = new Login();
                j.setLocationRelativeTo(null);
                j.setUndecorated(true);
                j.setResizable(false);
                j.setAlwaysOnTop(true);
                j.setLocationRelativeTo(null);
                j.show();
            }
        });

        MenuItem signOut = new MenuItem("Log Out");
        signOut.setOnAction(new EventHandler<ActionEvent>() {
            public void handle(ActionEvent e) {
                if (loggedIn == true) {
                    addLogoutMsg(innerChatVB, dude);
                    setUsername("");
                    setIp("");
                    setUrl("empty");
                    setPort(0);
                    setImgSelection(0);
                    setLoggedIn(false);
                    j.setUsername("");
                    j.setImgUrl("empty");
                    j.setHost("");
                    j.setFaceSelected(0);
                    j.setPort(0);
                }
            }
        });
        
        MenuItem exit = new MenuItem("Exit");
        exit.setOnAction(new EventHandler<ActionEvent>() {
            public void handle(ActionEvent t) {
                try {
                    System.out.println(j.getUsername() + " = username");
                    System.out.println(j.getImgUrl() + " = imgURL");
                    System.out.println(j.getHost() + " = hostIP");
                    System.out.println(j.getPort() + " = port");
                    System.out.println(j.getFaceSelected() + " = face selected");
                } catch (NullPointerException e) {

                }
                System.exit(0);
            }
        });

        file.getItems().addAll(signIn, signOut, exit);
        Menu edit = new Menu("Edit");

        MenuItem clr = new MenuItem("Clear Chat");
        clr.setOnAction(new EventHandler<ActionEvent>() {
            public void handle(ActionEvent e) {
                innerChatVB.getChildren().clear();
            }
        });

        edit.getItems().add(clr);

        Menu view = new Menu("View");
        menuBar.getMenus().addAll(file, edit, view);
        menuBar.autosize();

        VBox root = new VBox();
        root.setSpacing(10);
        root.setBackground(new Background(new BackgroundFill(Color.BLACK, CornerRadii.EMPTY, Insets.EMPTY)));

        root.getChildren().addAll(menuBar, chatAndUsersHB, inputAreaHB);
        Scene scene = new Scene(root, 690, 630);
        scene.getStylesheets().add(getClass().getResource("TreeStyle.css").toExternalForm());
        
        ta.requestFocus();

        applicationStage.getIcons().add(new Image("file:icon.png"));
        applicationStage.setTitle("Lazer Chat");
        applicationStage.setResizable(false);
        applicationStage.setScene(scene);
        applicationStage.show();

        scene.setOnMouseMoved(new EventHandler<MouseEvent>() {
            public void handle(MouseEvent m) {
                try {
                    setPort();
                } catch (Exception e) {
                }
                if (port == 0) {
                    setLoggedIn(false);
                } else {
                    setUsername();
                    setIp();
                    setUrl();
                    setPort();
                    setImgSelection();
                    if (loggedIn == false) {
                        Platform.runLater(new Runnable() {
                            @Override
                            public void run() {
                                addLoginMsg(innerChatVB, dude);
                                setLoggedIn(true);

                                Platform.runLater(() -> {
                                    try {
                                        sleep(500);
                                        dude.setVvalue(innerChatVB.getHeight());
                                        //new Thread(task).start();

                                    } catch (InterruptedException ex) {
                                        Logger.getLogger(Gateley_Stones_GroupProject_Client.class.getName()).log(Level.SEVERE, null, ex);
                                    }
                                });
                            }
                        });
                    }
                }
            }
        });
        
        ta.setOnMouseMoved(new EventHandler<MouseEvent>() {
            public void handle(MouseEvent m) {
                dude.setVvalue(innerChatVB.getHeight());
            }
        });
        userList.getSelectionModel().selectedItemProperty().addListener( new ChangeListener() {

            @Override
            public void changed(ObservableValue observable, Object oldValue,
                    Object newValue) {

                TreeItem<String> selectedItem = (TreeItem<String>) newValue;
                
                ta.clear();
                ta.appendText("/pm" + selectedItem.getValue());
                Platform.runLater(() -> requestFocus(ta));
                // do what ever you want 
            }

          });
        
    }

    public ScrollPane getDude() {
            return dude;
        }
    public void requestFocus(TextArea n) {
        n.requestFocus();
    }
    public void closeWindow(Stage s) {
        s.close();
    }

    public void setUsername() {
        username = j.getUsername();
    }

    public void setIp() {
        ip = j.getHost();
    }

    public void setUrl() {
        url = j.getImgUrl();
        if (url.equals("")) {
            url = "empty";
        }
    }

    public void setPort() {
        port = j.getPort();
    }

    public void setImgSelection() {
        imgSelection = j.getFaceSelected();
    }

    public void setUsername(String s) {
        username = s;
    }

    public void setIp(String s) {
        ip = s;
    }

    public void setUrl(String s) {
        url = s;
    }

    public void setPort(int i) {
        port = i;
    }

    public void setImgSelection(int i) {
        imgSelection = i;
    }

    public void setLoggedIn(boolean b) {
        loggedIn = b;
    }
    
    
    public HBox makeReceivedMessage(String text, String username, boolean usingPicturePreset, int imgSelection, String url, boolean isPM) {
        Ellipse e = new Ellipse(30, 30);
        
        arrow  = new ImageView(new Image("file:arrow.gif"));
        Image pic;
        
        if (usingPicturePreset) {
            pic = new Image(getUserPic(imgSelection));
        } else {
            pic = new Image(url);
        }
        
        e.setFill(new ImagePattern(pic));
        
        int fontSize;
        
        if (text.length() < 10) {
            fontSize = 50;
        }
        else if (text.length() < 39) {
            int diff = 39-(int)(text.length()*1.1);
            if (diff < 0) {
                diff = 0;
            }
            fontSize = 16 + diff;
        }
        else {
            fontSize = 16;
        }

        VBox vboxPicture = new VBox(e);
        vboxPicture.setAlignment(Pos.TOP_CENTER);
        vboxPicture.setPadding(new Insets(10));
        
        Label uName = new Label();
        uName.setText(username);
        uName.setTextFill(Color.CHARTREUSE);
        uName.setFont(Font.font("Helvetica", FontWeight.BOLD, 16));

        Label you = new Label("  " + this.username);
        you.setTextFill(Color.CHARTREUSE);
        you.setFont(Font.font("Helvetica", FontWeight.BOLD, 16));
        
        date = new Date();
        df.setTimeZone(TimeZone.getDefault());

        Label tStamp = new Label(df.format(date));
        tStamp.setTextFill(Color.SKYBLUE);
        tStamp.setFont(Font.font("Helvetica", FontWeight.BOLD, 12));

        Label mess = new Label(text);
        mess.setTextFill(Color.WHITE);
        mess.setFont(Font.font("Helvetica", FontWeight.BOLD, fontSize));
        mess.setWrapText(true);
        
        
        VBox vboxText;
        if(isPM) {
            uName.setText(username + "  ");
            HBox pm = new HBox(uName,arrow, you);
            pm.setAlignment(Pos.CENTER_LEFT);
            vboxText = new VBox(pm, tStamp, mess);
        }
        else {
            vboxText = new VBox(uName, tStamp, mess);
        }
        vboxText.setAlignment(Pos.TOP_LEFT);
        

        HBox hbox = new HBox(vboxPicture, vboxText);

        hbox.setAlignment(Pos.CENTER_LEFT);
        hbox.setMinWidth(480);
        hbox.setMaxWidth(480);
        hbox.setMinHeight(100);
        hbox.setStyle("-fx-background-color: #555555");

        return hbox;
    }

    public HBox makeLocalMessage(String text, String friendName, boolean usingPicturePreset, int imgSelection, String url, boolean isPM) {
        Ellipse e = new Ellipse(30, 30);

        arrow  = new ImageView(new Image("file:arrow.gif"));
        
        Image pic;

        if (usingPicturePreset) {
            pic = new Image(getUserPic(imgSelection));
        } else {
            pic = new Image(url);
        }
        
        int fontSize;
        
        if (text.length() < 10) {
            fontSize = 50;
        }
        else if (text.length() < 39) {
            int diff = 39-(int)(text.length()*1.1);
            if (diff < 0) {
                diff = 0;
            }
            fontSize = 16 + diff;
        }
        else {
            fontSize = 16;
        }
        
        e.setFill(new ImagePattern(pic));
        
        VBox vboxPicture = new VBox(e);
        vboxPicture.setAlignment(Pos.TOP_CENTER);
        vboxPicture.setPadding(new Insets(10));

        Label uName = new Label(this.username);
        uName.setTextFill(Color.GOLD);
        uName.setFont(Font.font("Helvetica", FontWeight.BOLD, 16));

        Label friend = new Label("  " + friendName);
        friend.setTextFill(Color.GOLD);
        friend.setFont(Font.font("Helvetica", FontWeight.BOLD, 16));
        
        date = new Date();
        df.setTimeZone(TimeZone.getDefault());

        Label tStamp = new Label(df.format(date));
        tStamp.setTextFill(Color.SKYBLUE);
        tStamp.setFont(Font.font("Helvetica", FontWeight.BOLD, 12));

        Label mess = new Label(text);
        mess.setTextFill(Color.WHITE);
        mess.setFont(Font.font("Helvetica", FontWeight.BOLD, fontSize));
        mess.setWrapText(true);
        mess.setTextAlignment(TextAlignment.RIGHT);
        if(text.length() > 39) {
            mess.setTextAlignment(TextAlignment.LEFT);
            mess.setPadding(new Insets(0,0,0,0));
        }
        
        VBox vboxText;
        if(isPM) {
            uName.setText(username + "  ");
            HBox pm = new HBox(uName,arrow, friend);
            pm.setAlignment(Pos.CENTER_RIGHT);
            vboxText = new VBox(pm, tStamp, mess);
        }
        else {
            vboxText = new VBox(uName, tStamp, mess);
        }
  
        vboxText.setAlignment(Pos.TOP_RIGHT);

        HBox hbox = new HBox(vboxText, vboxPicture);
        hbox.setMinWidth(480);
        hbox.setMaxWidth(480);
        hbox.setMinHeight(100);
        hbox.setStyle("-fx-background-color: #555555");
        hbox.setPadding(new Insets(0, 20, 0, 0));
        hbox.setAlignment(Pos.CENTER_RIGHT);

        return hbox;
    }
    
    
    public void addLoginMsg(VBox vb, ScrollPane s) {
        Label loginMsg = new Label("Signed In (" + username + ")");
        loginMsg.setTextFill(Color.GOLD);
        loginMsg.setFont(Font.font("Helvetica", FontWeight.BOLD, 20));
        date = new Date();
        df.setTimeZone(TimeZone.getDefault());
        Label tStamp = new Label(df.format(date));
        tStamp.setTextFill(Color.SKYBLUE);
        tStamp.setFont(Font.font("Helvetica", FontWeight.BOLD, 12));
        VBox mText = new VBox(loginMsg, tStamp);
        mText.setAlignment(Pos.TOP_RIGHT);

        HBox MsgHB = new HBox(mText);
        MsgHB.setMinSize(480, 100);
        MsgHB.setPrefSize(480, 100);
        MsgHB.setStyle("-fx-background-color: #555555");
        MsgHB.setPadding(new Insets(0, 20, 0, 0));
        MsgHB.setAlignment(Pos.CENTER_RIGHT);
        vb.getChildren().add(MsgHB);
        Platform.runLater(() -> {
            try {
                sleep(500);
                Platform.runLater(() -> s.setVvalue(vb.getHeight()));
            } catch (InterruptedException ex) {
                Logger.getLogger(Gateley_Stones_GroupProject_Client.class.getName()).log(Level.SEVERE, null, ex);
            }
        });
    }

    public void addLogoutMsg(VBox vb, ScrollPane s) {
        Label loginMsg = new Label("Logged Out (" + username + ")");
        loginMsg.setTextFill(Color.GOLD);
        loginMsg.setFont(Font.font("Helvetica", FontWeight.BOLD, 20));
        date = new Date();
        df.setTimeZone(TimeZone.getDefault());
        Label tStamp = new Label(df.format(date));
        tStamp.setTextFill(Color.SKYBLUE);
        tStamp.setFont(Font.font("Helvetica", FontWeight.BOLD, 12));
        VBox mText = new VBox(loginMsg, tStamp);
        mText.setAlignment(Pos.TOP_RIGHT);

        HBox MsgHB = new HBox(mText);
        MsgHB.setMinSize(480, 100);
        MsgHB.setPrefSize(480, 100);
        MsgHB.setStyle("-fx-background-color: #555555");
        MsgHB.setPadding(new Insets(0, 20, 0, 0));
        MsgHB.setAlignment(Pos.CENTER_RIGHT);
        vb.getChildren().add(MsgHB);
        Platform.runLater(() -> {
            try {
                sleep(500);
                Platform.runLater(() -> s.setVvalue(vb.getHeight()));
            } catch (InterruptedException ex) {
                Logger.getLogger(Gateley_Stones_GroupProject_Client.class.getName()).log(Level.SEVERE, null, ex);
            }
        });

    }

    public String getUserPic(int i) {
        switch (i) {
            case 1:
                return "file:pic1.png";
            case 2:
                return "file:pic2.png";
            case 3:
                return "file:pic3.png";
            case 4:
                return "file:pic4.png";
            case 5:
                return "file:pic5.png";
            case 6:
                return "file:pic6.png";
        }
        return "file:bigemojis/poop.gif";
    }
public HBox makeReceivedEmoji(String text, String username, boolean usingPicturePreset, int imgSelection, String url) {
        Ellipse e = new Ellipse(30, 30);
        Image pic;

        if (usingPicturePreset) {
            pic = new Image(getUserPic(imgSelection));
        } else {
            pic = new Image(url);
        }

        e.setFill(new ImagePattern(pic));

        VBox vboxPicture = new VBox(e);
        vboxPicture.setAlignment(Pos.TOP_CENTER);
        vboxPicture.setPadding(new Insets(10));

        Label uName = new Label(username);
        uName.setTextFill(Color.CHARTREUSE);
        uName.setFont(Font.font("Helvetica", FontWeight.BOLD, 16));

        date = new Date();
        df.setTimeZone(TimeZone.getDefault());

        Label tStamp = new Label(df.format(date));
        tStamp.setTextFill(Color.SKYBLUE);
        tStamp.setFont(Font.font("Helvetica", FontWeight.BOLD, 12));

        ImageView emoji = getImageViewBig(text);

        VBox vboxText = new VBox(uName, tStamp, emoji);
        vboxText.setAlignment(Pos.TOP_LEFT);

        HBox hbox = new HBox(vboxPicture, vboxText);

        hbox.setAlignment(Pos.CENTER_LEFT);
        hbox.setMinSize(480, 100);
        hbox.setMaxWidth(480);
        hbox.setStyle("-fx-background-color: #555555");

        return hbox;
    }
    private HBox sendEmoji(String imageName) {
        Ellipse e = new Ellipse(30, 30);
        Image pic = new Image(getUserPic(imgSelection));
        e.setFill(new ImagePattern(pic));
        VBox mPic = new VBox(e);
        mPic.setAlignment(Pos.TOP_CENTER);
        mPic.setPadding(new Insets(10));
        Label uName = new Label(username);
        uName.setTextFill(Color.GOLD);
        uName.setFont(Font.font("Helvetica", FontWeight.BOLD, 16));
        date = new Date();
        df.setTimeZone(TimeZone.getDefault());
        Label tStamp = new Label(df.format(date));
        tStamp.setTextFill(Color.SKYBLUE);
        tStamp.setFont(Font.font("Helvetica", FontWeight.BOLD, 12));
        ImageView emoji = getImageViewBig(imageName);
        VBox mText = new VBox(uName, tStamp, emoji);
        mText.setAlignment(Pos.TOP_RIGHT);

        HBox hbox = new HBox(mText, mPic);
        hbox.setMinWidth(480);
        hbox.setMaxWidth(480);
        hbox.setMinHeight(100);
        hbox.setStyle("-fx-background-color: #555555");
        hbox.setPadding(new Insets(0, 20, 0, 0));
        hbox.setAlignment(Pos.CENTER_RIGHT);
        return hbox;
    }

    private ImageView getImageViewBig(String imageName) throws NullPointerException {
        ImageView imageView = null;

        switch (imageName) {
            case "angry":
                imageView = new ImageView(new Image("file:bigemojis/" + imageName + ".gif"));
                break;
            case "awwyeah":
                imageView = new ImageView(new Image("file:bigemojis/" + imageName + ".gif"));
                break;
            case "brokenheart":
                imageView = new ImageView(new Image("file:bigemojis/" + imageName + ".gif"));
                break;
            case "clapping":
                imageView = new ImageView(new Image("file:bigemojis/" + imageName + ".gif"));
                break;
            case "dislike":
                imageView = new ImageView(new Image("file:bigemojis/" + imageName + ".gif"));
                break;
            case "facepalm":
                imageView = new ImageView(new Image("file:bigemojis/" + imageName + ".gif"));
                break;
            case "happy":
                imageView = new ImageView(new Image("file:bigemojis/" + imageName + ".gif"));
                break;
            case "headbang":
                imageView = new ImageView(new Image("file:bigemojis/" + imageName + ".gif"));
                break;
            case "heart":
                imageView = new ImageView(new Image("file:bigemojis/" + imageName + ".gif"));
                break;
            case "hi":
                imageView = new ImageView(new Image("file:bigemojis/" + imageName + ".gif"));
                break;
            case "idea":
                imageView = new ImageView(new Image("file:bigemojis/" + imageName + ".gif"));
                break;
            case "kiss":
                imageView = new ImageView(new Image("file:bigemojis/" + imageName + ".gif"));
                break;
            case "laugh":
                imageView = new ImageView(new Image("file:bigemojis/" + imageName + ".gif"));
                break;
            case "like":
                imageView = new ImageView(new Image("file:bigemojis/" + imageName + ".gif"));
                break;
            case "poop":
                imageView = new ImageView(new Image("file:bigemojis/" + imageName + ".gif"));
                break;
            case "punch":
                imageView = new ImageView(new Image("file:bigemojis/" + imageName + ".gif"));
                break;
            case "rock":
                imageView = new ImageView(new Image("file:bigemojis/" + imageName + ".gif"));
                break;
            case "sad":
                imageView = new ImageView(new Image("file:bigemojis/" + imageName + ".gif"));
                break;
            case "wow":
                imageView = new ImageView(new Image("file:bigemojis/" + imageName + ".gif"));
                break;
            case "wtf":
                imageView = new ImageView(new Image("file:bigemojis/" + imageName + ".gif"));
                break;
        }
        return imageView;
    }

    static class StringImageCell extends ListCell<String> {

        Label label;

        @Override
        protected void updateItem(String item, boolean empty) {
            super.updateItem(item, empty);
            if (item == null || empty) {
                setItem(null);
                setGraphic(null);
            } else {
                setText(item);
                ImageView image = getImageView(item);
                label = new Label("", image);
                setGraphic(label);
            }
        }

    }

    private static ImageView getImageView(String imageName) throws NullPointerException {
        ImageView imageView = null;
        switch (imageName) {
            case "angry":
                imageView = new ImageView(new Image("file:emojis/" + imageName + ".gif"));
                break;
            case "awwyeah":
                imageView = new ImageView(new Image("file:emojis/" + imageName + ".gif"));
                break;
            case "brokenheart":
                imageView = new ImageView(new Image("file:emojis/" + imageName + ".gif"));
                break;
            case "clapping":
                imageView = new ImageView(new Image("file:emojis/" + imageName + ".gif"));
                break;
            case "dislike":
                imageView = new ImageView(new Image("file:emojis/" + imageName + ".gif"));
                break;
            case "facepalm":
                imageView = new ImageView(new Image("file:emojis/" + imageName + ".gif"));
                break;
            case "happy":
                imageView = new ImageView(new Image("file:emojis/" + imageName + ".gif"));
                break;
            case "headbang":
                imageView = new ImageView(new Image("file:emojis/" + imageName + ".gif"));
                break;
            case "heart":
                imageView = new ImageView(new Image("file:emojis/" + imageName + ".gif"));
                break;
            case "hi":
                imageView = new ImageView(new Image("file:emojis/" + imageName + ".gif"));
                break;
            case "idea":
                imageView = new ImageView(new Image("file:emojis/" + imageName + ".gif"));
                break;
            case "kiss":
                imageView = new ImageView(new Image("file:emojis/" + imageName + ".gif"));
                break;
            case "laugh":
                imageView = new ImageView(new Image("file:emojis/" + imageName + ".gif"));
                break;
            case "like":
                imageView = new ImageView(new Image("file:emojis/" + imageName + ".gif"));
                break;
            case "poop":
                imageView = new ImageView(new Image("file:emojis/" + imageName + ".gif"));
                break;
            case "punch":
                imageView = new ImageView(new Image("file:emojis/" + imageName + ".gif"));
                break;
            case "rock":
                imageView = new ImageView(new Image("file:emojis/" + imageName + ".gif"));
                break;
            case "sad":
                imageView = new ImageView(new Image("file:emojis/" + imageName + ".gif"));
                break;
            case "wow":
                imageView = new ImageView(new Image("file:emojis/" + imageName + ".gif"));
                break;
            case "wtf":
                imageView = new ImageView(new Image("file:emojis/" + imageName + ".gif"));
                break;
        }
        return imageView;
    }

}
