package gateley_stones_groupproject_client;

import javafx.application.Platform;
import javafx.application.Preloader;
import javafx.geometry.Pos;
import javafx.scene.Scene;
import javafx.scene.control.Label;
import javafx.scene.image.Image;
import javafx.scene.image.ImageView;
import javafx.scene.layout.StackPane;
import javafx.scene.text.TextAlignment;
import javafx.stage.Stage;
import javafx.stage.StageStyle;

public class MyPreloader extends Preloader {

    private static final double WIDTH = 1175;
    private static final double HEIGHT = 580;

    private Stage preloaderStage;
    private Scene scene;

    private Label progress;

    public MyPreloader() {
    }

    @Override
    public void init() throws Exception {
        // If preloader has complex UI it's initialization can be done in MyPreloader#init
        Platform.runLater(() -> {
            Label title = new Label("Showing preloader stage!\nLoading, please wait...");
            title.setTextAlignment(TextAlignment.CENTER);
            title.setStyle("-fx-text-fill: rgb(0,100,0)");
            progress = new Label("0%");
            progress.setVisible(false);
            StackPane root = new StackPane();
            root.setBackground(null);
            root.setAlignment(Pos.CENTER);
            ImageView logo = new ImageView(new Image("file:logo.gif"));
            ImageView bg = new ImageView(new Image("file:backgeo6.png"));
            logo.setTranslateX(5);
            root.getChildren().addAll(bg,logo, progress);
            scene = new Scene(root, WIDTH, HEIGHT);
            scene.setFill(null);
        });
    }
    
    @Override
    public void start(Stage primaryStage) throws Exception {
        this.preloaderStage = primaryStage;
        preloaderStage.initStyle(StageStyle.TRANSPARENT);
        // Set preloader scene and show stage.
        preloaderStage.setScene(scene);
        preloaderStage.show();
        
    }

    @Override
    public void handleApplicationNotification(PreloaderNotification info) {
        // Handle application notification in this point (see MyApplication#init).
        if (info instanceof ProgressNotification) {
            progress.setText(((ProgressNotification) info).getProgress() + "%");
        }
        if(progress.getText().equals("99.0%")) {
           preloaderStage.close();
        }
    }

    
}
