package stones_jeremiah_lab9_javafx_mt;

import java.io.BufferedReader;
import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileReader;
import java.io.IOException;
import static java.lang.Thread.sleep;
import java.util.Random;
import java.util.logging.Level;
import java.util.logging.Logger;
import javafx.application.Application;
import javafx.event.ActionEvent;
import javafx.event.EventHandler;
import javafx.geometry.Insets;
import javafx.geometry.Pos;
import javafx.scene.Scene;
import javafx.scene.chart.BarChart;
import javafx.scene.chart.CategoryAxis;
import javafx.scene.chart.NumberAxis;
import javafx.scene.chart.XYChart;
import javafx.scene.control.Button;
import javafx.scene.control.Label;
import javafx.scene.control.TextField;
import javafx.scene.effect.ImageInput;
import javafx.scene.image.Image;
import javafx.scene.image.ImageView;
import javafx.scene.input.MouseEvent;
import javafx.scene.layout.Background;
import javafx.scene.layout.BackgroundFill;
import javafx.scene.layout.CornerRadii;
import javafx.scene.layout.HBox;
import javafx.scene.layout.Priority;
import javafx.scene.layout.VBox;
import javafx.scene.paint.Color;
import javafx.scene.shape.Circle;
import javafx.stage.FileChooser;
import javafx.stage.FileChooser.ExtensionFilter;
import javafx.stage.Stage;

// The submit and run test buttons automatically clear the data from the graph as well.
// The program will use a different interval for checking to
// see if the threading is done, dependent on how many numbers it's sorting.
// The full screen button will appear if a user gets out of full screen
class SuperThreader implements Runnable {

    public static int num;
    final private String name;
    final private SubThreader subby;
    final private int arr[];
    private int arr2[];
    Thread t = new Thread();
    public Thread getT() {
        return t;
    }
    SuperThreader(String name, int arr[]) {
        this.name = name;
        this.arr = arr;
        subby = new SubThreader(this.arr);
        t = new Thread(this, name);
        t.start();
    }

    @Override
    public void run() {
        this.arr2 = merge_sort(subby.getMySubA()).clone();
    }

    public String getName() {
        return this.name;
    }

    

    public int[] getArr2() {
        return arr2;
    }

    public static int[] merge_sort(int[] A) {
        if (A.length <= 1) {
            return A;
        }
        int length = A.length;
        int half = length / 2;
        int[] left = new int[half];
        int[] right;
        int leftCnt = 0;
        int rightCnt = 0;
        if (length % 2 == 0) {
            right = new int[half];
        } else {
            right = new int[half + 1];
        }
        for (int i = 0; i < length; i++) {
            if (i % 2 == 0) {
                right[rightCnt++] = A[i];
            } else {
                left[leftCnt++] = A[i];
            }
        }

        left = merge_sort(left);
        right = merge_sort(right);
        return merge(left, right);
    }

    public static int[] merge(int[] l, int[] r) {
        int result[] = new int[l.length + r.length];
        int lCnt = 0;
        int rCnt = 0;
        int resCnt = 0;
        while (lCnt < l.length || rCnt < r.length) {
            while (lCnt < l.length && rCnt < r.length) {
                if (l[lCnt] <= r[rCnt]) {
                    result[resCnt++] = l[lCnt++];
                } else {
                    result[resCnt++] = r[rCnt++];
                }
            }
            if (lCnt < l.length) {
                for (int i = resCnt; i < result.length; i++) {
                    result[resCnt++] = l[lCnt++];
                }
            } else {
                for (int i = resCnt; i < result.length; i++) {
                    result[resCnt++] = r[rCnt++];
                }
            }
        }
        return result;
    }
}

class SubThreader {

    final private int mySubA[];

    SubThreader(int mySubA[]) {
        this.mySubA = mySubA;
    }

    public int[] getMySubA() {
        return this.mySubA;
    }
}

class Sort {
    final private double oldData[] = new double[20];
    private int arr2Sort[];
    private double mtSortTime[];
    private double regSortTime[];
    private final int processors = Runtime.getRuntime().availableProcessors(); // get number of processors and store in variable
    
    double[] getMTSortTime() {
        return mtSortTime;
    }

    double[] getRegSortTime() {
        return regSortTime;
    }

    int getProcessors() {
        return processors;
    }
    
    double[] getOldData(){
        return oldData;
    }
    
    void setOldData(File file) throws IOException {
        FileReader fr = new FileReader(file);
        BufferedReader br = new BufferedReader(fr);
        String str;
        for(int i = 0; i < 20; i++){
            str = br.readLine();
            double d;
            d = Double.parseDouble(str);
            oldData[i] = d;
        }
        
    }
    
    void doTest(int numOfNums) throws InterruptedException {
        long startTime, stopTime;
        double sortTime;
        this.mtSortTime = new double[10];
        this.regSortTime = new double[10];
        Random rand = new Random();
        int myA[] = new int[numOfNums]; // main array of random numbers
        SuperThreader st[] = new SuperThreader[processors]; // array of threads
        int sa[][] = new int[processors][]; //SubArrays[] "sa" of myA[] (my array)
        int l[] = new int[this.processors]; // create an array for subArray, sa[], length, based on number of processors
        int i, j; // loop variables declared now to use multiple variables in for loop args

        for (i = 0, j = processors; i < processors; i++, j--) {
            if (i == 0) {
                l[i] = myA.length / processors;
            } else if (myA.length % processors == j) {
                l[i] = myA.length / processors + 1;
            } else {
                l[i] = myA.length / processors;
            }
            sa[i] = new int[l[i]]; // also using this loop to create a new array of length, l[i], for each element in sa[]
        }
        ////// loop this part for testing 10 different arrays with both sorts //////
        for (int loopy = 0; loopy < 10; loopy++) {
            for (i = 0; i < myA.length; i++) {
                myA[i] = rand.nextInt(10000); // random numbers fill main array, myA[]
            }
            arr2Sort = myA.clone();
            System.out.println("Multithreaded Merge-Sort");
            System.out.println("Number of elements in int array: " + myA.length);
            System.out.print("Middle 10 elements before sort: ");
            for (i = 0; i < 10; i++) {
                System.out.print(myA[myA.length / 2 - 5 + i] + " ");
            }
            System.out.println();
            startTime = System.nanoTime();
            try {
                for (i = 0; i < (myA.length / processors); i++) {
                    int len = 0;
                    for (j = 0; j < processors; j++) {
                        if (j == 0) {
                            sa[j][i] = myA[i];
                            len += sa[j].length;
                        } else {
                            sa[j][i] = myA[i + len];
                            len += sa[j].length;
                        }
                    }
                }
            } catch (ArrayIndexOutOfBoundsException a) {
            }

            for (i = 0; i < processors; i++) {
                String tNum = "Thread " + String.valueOf(i + 1);
                st[i] = new SuperThreader(tNum, sa[i]);
            }

            while (true) {
                boolean alive = false;
                if (numOfNums >= 1000000) {
                    sleep(100);
                } else if (numOfNums >= 500000) {
                    sleep(50);
                } else if (numOfNums >= 100000) {
                    sleep(10);
                } else if (numOfNums >= 50000) {
                    sleep(5);
                } else {
                    sleep(1);
                }

                for (SuperThreader x : st) {
                    if (x.getT().isAlive()) {
                        alive = true;
                    }
                }
                if (alive == false) {
                    break;
                }
            }

            try {
                for (i = 0; i < (myA.length / processors); i++) {
                    int len = 0;
                    for (j = 0; j < processors; j++) {
                        if (j == 0) {
                            myA[i] = sa[j][i];
                            len += sa[j].length;
                        } else {
                            myA[i + len] = sa[j][i];
                            len += sa[j].length;
                        }
                    }
                }
            } catch (ArrayIndexOutOfBoundsException a) {
            }

            int reMerge[][] = new int[processors - 1][];

            for (i = 0; i < reMerge.length; i++) {
                if (i == 0) {
                    reMerge[i] = new int[st[i].getArr2().length + st[i + 1].getArr2().length];
                    reMerge[i] = SuperThreader.merge(st[i].getArr2(), st[i + 1].getArr2()).clone();
                } else {
                    reMerge[i] = new int[reMerge[i - 1].length + st[i + 1].getArr2().length];
                    reMerge[i] = SuperThreader.merge(reMerge[i - 1], st[i + 1].getArr2()).clone();
                }
            }
            stopTime = System.nanoTime();
            myA = reMerge[processors - 2].clone();
            System.out.println("Number of threads used: " + processors);
            System.out.print("Middle 10 elements after sort: ");
            for (i = 0; i < 10; i++) {
                System.out.print(myA[(myA.length / 2) - 5 + i] + " ");
            }
            sortTime = (double) ((stopTime - startTime) / 1000000000d);
            mtSortTime[loopy] = sortTime;
            System.out.println("\nSort timer reading: " + sortTime + " seconds");
            System.out.println();

            ///////////////////// Regular Merge Part //////////////
            System.out.println("Regular Merge-Sort");
            System.out.println("Number of elements in int array: " + arr2Sort.length);
            System.out.print("Middle 10 elements before sort: ");
            for (i = 0; i < 10; i++) {
                System.out.print(arr2Sort[arr2Sort.length / 2 - 5 + i] + " ");
            }
            System.out.println();
            startTime = System.nanoTime();
            arr2Sort = SuperThreader.merge_sort(arr2Sort);
            stopTime = System.nanoTime();

            System.out.print("Middle 10 elements after sort: ");

            for (i = 0; i < 10; i++) {
                System.out.print(arr2Sort[arr2Sort.length / 2 - 5 + i] + " ");
            }
            sortTime = (double) ((stopTime - startTime) / 1000000000d);
            regSortTime[loopy] = sortTime;
            System.out.println("\nSort timer reading: " + sortTime + " seconds");
            System.out.println();
        }
    }
}

public class Stones_Jeremiah_Lab9_JavaFX_MT extends Application {

    double threadTimes[] = new double[10];
    double regTimes[] = new double[10];
    double oldStuff[];
    @Override
    public void start(Stage primaryStage) throws FileNotFoundException, IOException {
        SuperThreader.num = 0;
        Button testBtn = new Button("      Run\nComparison\n      Test");
        Button oldTestBtn = new Button("Run\n Old\nData");
        Button toFullScreenBtn = new Button("Full Screen Mode");
        Button exitBtn = new Button("Exit Program");
        Button clearDataBtn = new Button("Clear\nData");
        Button oldBtn = new Button("Load Old Data From File");
        Button newBtn = new Button("Input New Data Set");
        Circle circ4btn = new Circle();
        circ4btn.setRadius(60);
        testBtn.setShape(circ4btn);
        oldTestBtn.setShape(circ4btn);
        clearDataBtn.setShape(circ4btn);
        Sort sort = new Sort();
        FileChooser fc = new FileChooser();
        fc.setTitle("Open Resource File");
        fc.setInitialDirectory(new File(System.getProperty("user.dir")));
        fc.getExtensionFilters().add(
        new ExtensionFilter("Text Files", "*.txt"));        
        Label l = new Label("How many\nnumbers\nto sort?\n\nSubmit\nthen run.\n\n"
                + "Must be between\n20 and 25000000.");
        l.setAlignment(Pos.CENTER);
        Button submit = new Button("SUBMIT");
        TextField tf = new TextField();
        Label l2 = new Label("\nProcessors: " + sort.getProcessors() + "\n");
        VBox howMany = new VBox(20, oldBtn,newBtn);
        howMany.setPrefWidth(250);
        CategoryAxis x = new CategoryAxis();
        NumberAxis y = new NumberAxis();
        BarChart<String, Number> bc
                = new BarChart<>(x, y);
        bc.setTitle("Sort Test Summary");
        x.setLabel("Trial Array");
        y.setLabel("Time in Seconds");

        XYChart.Series mtSeries = new XYChart.Series();
        mtSeries.setName("MT Sort");
        XYChart.Series regSeries = new XYChart.Series();
        regSeries.setName("Reg Sort");        
        bc.setAnimated(false);
        
        
        ImageInput runMan = new ImageInput(new Image("file:resources/glob.gif"));
        ImageInput runMan2 = new ImageInput(new Image("file:resources/glob.gif"));
        ImageInput doom = new ImageInput(new Image("file:resources/clearblob.gif"));
        testBtn.setOnMouseEntered(new EventHandler <MouseEvent>() {
            public void handle(MouseEvent m) {
                testBtn.setEffect(runMan);
            }
        } 
        );
        testBtn.setOnMouseExited(new EventHandler <MouseEvent>() {
            public void handle(MouseEvent m) {
                testBtn.setEffect(null);
        } 
        });
        oldTestBtn.setOnMouseEntered(new EventHandler <MouseEvent>() {
            public void handle(MouseEvent m) {
                oldTestBtn.setEffect(runMan2);
            }
        } 
        );
        oldTestBtn.setOnMouseExited(new EventHandler <MouseEvent>() {
            public void handle(MouseEvent m) {
                oldTestBtn.setEffect(null);
        } 
        });
        oldTestBtn.setOnAction(new EventHandler <ActionEvent>(){
            @Override
            public void handle(ActionEvent a) {
                
                oldStuff = sort.getOldData();
                mtSeries.getData().clear();
                regSeries.getData().clear();
                mtSeries.getData().add(new XYChart.Data("Array 1", oldStuff[0]));
                mtSeries.getData().add(new XYChart.Data("Array 2", oldStuff[2]));
                mtSeries.getData().add(new XYChart.Data("Array 3", oldStuff[4]));
                mtSeries.getData().add(new XYChart.Data("Array 4", oldStuff[6]));
                mtSeries.getData().add(new XYChart.Data("Array 5", oldStuff[8]));
                mtSeries.getData().add(new XYChart.Data("Array 6", oldStuff[10]));
                mtSeries.getData().add(new XYChart.Data("Array 7", oldStuff[12]));
                mtSeries.getData().add(new XYChart.Data("Array 8", oldStuff[14]));
                mtSeries.getData().add(new XYChart.Data("Array 9", oldStuff[16]));
                mtSeries.getData().add(new XYChart.Data("Array 10", oldStuff[18]));
                regSeries.getData().add(new XYChart.Data("Array 1", oldStuff[1]));
                regSeries.getData().add(new XYChart.Data("Array 2", oldStuff[3]));
                regSeries.getData().add(new XYChart.Data("Array 3", oldStuff[5]));
                regSeries.getData().add(new XYChart.Data("Array 4", oldStuff[7]));
                regSeries.getData().add(new XYChart.Data("Array 5", oldStuff[9]));
                regSeries.getData().add(new XYChart.Data("Array 6", oldStuff[11]));
                regSeries.getData().add(new XYChart.Data("Array 7", oldStuff[13]));
                regSeries.getData().add(new XYChart.Data("Array 8", oldStuff[15]));
                regSeries.getData().add(new XYChart.Data("Array 9", oldStuff[17]));
                regSeries.getData().add(new XYChart.Data("Array 10", oldStuff[19]));
                
                oldTestBtn.setVisible(false);
                newBtn.setVisible(true);
                oldBtn.setVisible(true);
                
            }
        });
        clearDataBtn.setOnMouseEntered(new EventHandler <MouseEvent>() {
            public void handle(MouseEvent m) {
                clearDataBtn.setEffect(doom);
            }
        } 
        );
        clearDataBtn.setOnMouseExited(new EventHandler <MouseEvent>() {
            public void handle(MouseEvent m) {
                clearDataBtn.setEffect(null);
        } 
        });
        testBtn.setOnAction(new EventHandler<ActionEvent>() {
            @Override
            public void handle(ActionEvent a) {
                if (SuperThreader.num < 20 || SuperThreader.num > 25000000) {
                } else {
                    
                    mtSeries.getData().clear();
                    regSeries.getData().clear();
                    try {
                        sort.doTest(SuperThreader.num);
                    } catch (InterruptedException ex) {
                    }
                    
                    threadTimes = sort.getMTSortTime().clone();
                    regTimes = sort.getRegSortTime().clone();
                    for(int j = 0; j<10; j++) {
                        System.out.println(threadTimes[j]);
                    }
                    System.out.println();
                    for(int j = 0; j<10; j++) {
                        System.out.println(regTimes[j]);
                    }
                    mtSeries.getData().add(new XYChart.Data("Array 1", threadTimes[0]));
                    mtSeries.getData().add(new XYChart.Data("Array 2", threadTimes[1]));
                    mtSeries.getData().add(new XYChart.Data("Array 3", threadTimes[2]));
                    mtSeries.getData().add(new XYChart.Data("Array 4", threadTimes[3]));
                    mtSeries.getData().add(new XYChart.Data("Array 5", threadTimes[4]));
                    mtSeries.getData().add(new XYChart.Data("Array 6", threadTimes[5]));
                    mtSeries.getData().add(new XYChart.Data("Array 7", threadTimes[6]));
                    mtSeries.getData().add(new XYChart.Data("Array 8", threadTimes[7]));
                    mtSeries.getData().add(new XYChart.Data("Array 9", threadTimes[8]));
                    mtSeries.getData().add(new XYChart.Data("Array 10", threadTimes[9]));
                    regSeries.getData().add(new XYChart.Data("Array 1", regTimes[0]));
                    regSeries.getData().add(new XYChart.Data("Array 2", regTimes[1]));
                    regSeries.getData().add(new XYChart.Data("Array 3", regTimes[2]));
                    regSeries.getData().add(new XYChart.Data("Array 4", regTimes[3]));
                    regSeries.getData().add(new XYChart.Data("Array 5", regTimes[4]));
                    regSeries.getData().add(new XYChart.Data("Array 6", regTimes[5]));
                    regSeries.getData().add(new XYChart.Data("Array 7", regTimes[6]));
                    regSeries.getData().add(new XYChart.Data("Array 8", regTimes[7]));
                    regSeries.getData().add(new XYChart.Data("Array 9", regTimes[8]));
                    regSeries.getData().add(new XYChart.Data("Array 10", regTimes[9]));
                    
                testBtn.setVisible(false);
                newBtn.setVisible(true);
                oldBtn.setVisible(true);
                
                howMany.getChildren().removeAll(l, tf, submit, l2);
                }
            }
        });
        clearDataBtn.setOnAction(new EventHandler<ActionEvent>() {
            @Override
            public void handle(ActionEvent a) {
                mtSeries.getData().clear();
                regSeries.getData().clear();
            }
        });
        exitBtn.setOnAction(new EventHandler<ActionEvent>() {
            @Override
            public void handle(ActionEvent a) {
                System.exit(0);
            }
        });
        ImageInput byebye = new ImageInput(new Image("file:resources/fire.gif"));
        exitBtn.setOnMouseEntered(new EventHandler <MouseEvent>() {
            public void handle(MouseEvent m) {
                exitBtn.setEffect(byebye);
            }
        } 
        );
        
        exitBtn.setOnMouseExited(new EventHandler <MouseEvent>() {
            public void handle(MouseEvent m) {
                exitBtn.setEffect(null);
        } 
        });
        toFullScreenBtn.setOnAction(new EventHandler<ActionEvent>() {
            @Override
            public void handle(ActionEvent a) {
                primaryStage.setFullScreen(true);
            }
        });
        toFullScreenBtn.setShape(circ4btn);
        ImageInput expand = new ImageInput(new Image("file:resources/expand.gif"));
        toFullScreenBtn.setOnMouseEntered(new EventHandler <MouseEvent>() {
            public void handle(MouseEvent m) {
                toFullScreenBtn.setEffect(expand);
            }
        } 
        );
        toFullScreenBtn.setOnMouseExited(new EventHandler <MouseEvent>() {
            public void handle(MouseEvent m) {
                toFullScreenBtn.setEffect(null);
        } 
        });
        
        Image title = new Image("file:resources/mainTitle.gif");
        ImageView titleIV = new ImageView(title);
        toFullScreenBtn.setPrefSize(150, 150);
        toFullScreenBtn.setBackground(new Background(new BackgroundFill(Color.CHARTREUSE, CornerRadii.EMPTY, new Insets(0))));
        testBtn.setPrefSize(120, 120);
        oldTestBtn.setPrefSize(120, 120);
        clearDataBtn.setPrefSize(120, 120);
        exitBtn.setPrefSize(150, 150);
        exitBtn.setBackground(new Background(new BackgroundFill(Color.RED, CornerRadii.EMPTY, new Insets(0))));
        exitBtn.setShape(circ4btn);
        Background mainBG = new Background(new BackgroundFill(Color.WHITE, CornerRadii.EMPTY, new Insets(0)));
        VBox vbRow1Col1 = new VBox();
        vbRow1Col1.setMinSize(150, 162);
        HBox hbRow1Col2 = new HBox();
        hbRow1Col2.setPrefSize(50, 162);
        HBox.setHgrow(hbRow1Col2, Priority.ALWAYS);
        VBox vbRow1Col3 = new VBox(titleIV);
        HBox hbRow1Col4 = new HBox();
        hbRow1Col4.setPrefSize(150, 162);
        HBox.setHgrow(hbRow1Col4, Priority.ALWAYS);
        VBox vbRow2Col1Row1 = new VBox(50, testBtn);
        testBtn.setVisible(false);
        vbRow2Col1Row1.setBackground(new Background(new BackgroundFill(Color.BLUEVIOLET, CornerRadii.EMPTY, new Insets(0))));
        vbRow2Col1Row1.setPrefSize(150, 200);
        vbRow2Col1Row1.setAlignment(Pos.CENTER);
        VBox.setVgrow(vbRow2Col1Row1, Priority.ALWAYS);
        VBox vbRow2Col1Row2 = new VBox(50, clearDataBtn);
        vbRow2Col1Row2.setBackground(new Background(new BackgroundFill(Color.DARKGREEN, CornerRadii.EMPTY, new Insets(0))));
        vbRow2Col1Row2.setPrefSize(150, 200);
        vbRow2Col1Row2.setAlignment(Pos.CENTER);
        VBox.setVgrow(vbRow2Col1Row1, Priority.ALWAYS);
        VBox vbRow2Col1 = new VBox(vbRow2Col1Row1, vbRow2Col1Row2);
        newBtn.setOnAction(new EventHandler <ActionEvent>() {
            @Override
            public void handle(ActionEvent a) {
                howMany.getChildren().addAll(l, tf, submit, l2);
                vbRow2Col1Row1.getChildren().remove(oldTestBtn);
                oldBtn.setVisible(false);
            }
        });
        submit.setOnAction(new EventHandler<ActionEvent>() {
            @Override
            public void handle(ActionEvent a) {
                mtSeries.getData().clear();
                regSeries.getData().clear();
                SuperThreader.num = Integer.valueOf(tf.getText());
                
                testBtn.setVisible(true);
                vbRow2Col1Row1.getChildren().add(testBtn);
            }
        });
        oldBtn.setOnAction(new EventHandler <ActionEvent>() {
           @Override
           public void handle(ActionEvent a) {
               //fc.setInitialDirectory(new File("C:/Users/Old Data/"));
               File selectedFile = fc.showOpenDialog(primaryStage);
               try {
                   sort.setOldData(selectedFile);
                   newBtn.setVisible(false);
                    howMany.getChildren().removeAll(l, tf, submit, l2);
                    vbRow2Col1Row1.getChildren().remove(testBtn);
                    vbRow2Col1Row1.getChildren().add(oldTestBtn);
               } catch (IOException ex) {
                   Logger.getLogger(Stones_Jeremiah_Lab9_JavaFX_MT.class.getName()).log(Level.SEVERE, null, ex);
               }
           } 
        });
        howMany.setPadding(new Insets(20));
        bc.setPadding(new Insets(0, 0, 0, 0));
        HBox hbRow2Col2 = new HBox(howMany, bc);
        hbRow2Col2.setAlignment(Pos.CENTER_LEFT);
        hbRow2Col2.setBackground(new Background(new BackgroundFill(Color.GOLD, CornerRadii.EMPTY, new Insets(0))));
        HBox.setHgrow(hbRow2Col2, Priority.ALWAYS);
        HBox hbRow1 = new HBox(vbRow1Col1, hbRow1Col2, vbRow1Col3, hbRow1Col4);
        hbRow1.setAlignment(Pos.CENTER);

        HBox hbRow2 = new HBox(vbRow2Col1, hbRow2Col2);
        HBox hbRow3 = new HBox(exitBtn);
        hbRow3.setAlignment(Pos.CENTER_RIGHT);
        VBox outerVBox = new VBox(hbRow1, hbRow2, hbRow3);
        VBox root = new VBox();
        bc.getData().addAll(mtSeries, regSeries);
        root.getChildren().addAll(outerVBox);
        root.setBackground(mainBG);
        root.setOnMouseMoved(new EventHandler<MouseEvent>() {
            @Override
            public void handle(MouseEvent m) {
                if (primaryStage.isFullScreen()) {
                    vbRow1Col1.getChildren().remove(toFullScreenBtn);
                } else {
                    vbRow1Col1.getChildren().add(toFullScreenBtn);
                }
            }
        }
        );

        Scene scene = new Scene(root, 800, 600);
        primaryStage.setTitle("Multithreaded Merge-Sort VS. Regular Merge-Sort");
        primaryStage.setScene(scene);
        primaryStage.setFullScreen(true);
        primaryStage.show();
    }

    /**
     * @param args the command line arguments
     */
    public static void main(String[] args) {
        launch(args);
    }

}
