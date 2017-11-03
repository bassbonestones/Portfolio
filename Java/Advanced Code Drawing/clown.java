/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package stones_jeremiah_clown;

import java.awt.BasicStroke;
import java.awt.Color;
import java.awt.Dimension;
import java.awt.Font;
import java.awt.Graphics;
import java.awt.Graphics2D;
import java.awt.Polygon;
import java.awt.Rectangle;
import java.awt.Toolkit;
import java.awt.geom.CubicCurve2D;
import java.awt.image.BufferedImage;
import javafx.application.Application;
import javafx.embed.swing.SwingFXUtils;
import javafx.event.ActionEvent;
import javafx.event.EventHandler;
import javafx.scene.Group;
import javafx.scene.Scene;
import javafx.scene.canvas.Canvas;
import javafx.scene.canvas.GraphicsContext;
import javafx.scene.control.Button;
import javafx.scene.image.Image;
import javafx.scene.layout.StackPane;
import javafx.stage.Stage;

/**
 *
 * @author Jeremy
 */
public class Stones_Jeremiah_Clown extends Application {
    
    @Override
    public void start(Stage primaryStage) {
        primaryStage.setTitle("Advanced Drawing With CODE!");
        Group root;
        root = new Group();
        Canvas canvas = new Canvas(650, 640);
        GraphicsContext gc = canvas.getGraphicsContext2D();
        paint(gc);
        root.getChildren().add(canvas);
        primaryStage.setScene(new Scene(root));
        primaryStage.show();
    }

    /**
     * @param args the command line arguments
     */
    public static void main(String[] args) {
        launch(args);
    }
    
    public void paint(GraphicsContext g) {
        Dimension dim = Toolkit.getDefaultToolkit().getScreenSize();
        //this.setSize(500,500);
        
        
        BufferedImage img = new BufferedImage(800,900,BufferedImage.TYPE_INT_RGB);
        
        Graphics2D G = img.createGraphics();
        G.setColor(Color.WHITE);
        Rectangle r = new Rectangle(0,0,dim.width,dim.height);
        G.fill(r);
        
        G.setColor(Color.BLACK);
        CubicCurve2D oHair1 = new CubicCurve2D.Double();
        oHair1.setCurve(237,632,210,668,162,672,145,646);
        CubicCurve2D iHair1 = new CubicCurve2D.Double();
        iHair1.setCurve(237,632,209,658,164,665,147,645);
        CubicCurve2D bHair1 = new CubicCurve2D.Double();
        bHair1.setCurve(237,632,209,662,164,660,147,645);
        
        CubicCurve2D oHair2 = new CubicCurve2D.Double();
        oHair2.setCurve(149,650,122,656,105,630,118,602);
        CubicCurve2D iHair2 = new CubicCurve2D.Double();
        iHair2.setCurve(147,647,128,653,110,630,122,604);
        CubicCurve2D bHair2 = new CubicCurve2D.Double();
        bHair2.setCurve(147,647,128,647,115,630,132,600);
        
        CubicCurve2D oHair3 = new CubicCurve2D.Double();
        oHair3.setCurve(120,613,67,595,56,555,91,510);
        CubicCurve2D iHair3 = new CubicCurve2D.Double();
        iHair3.setCurve(120,606,77,594,64,543,96,517);
        CubicCurve2D bHair3 = new CubicCurve2D.Double();
        bHair3.setCurve(132,600,95,625,74,543,107,523);
        
        CubicCurve2D oHair4 = new CubicCurve2D.Double();
        oHair4.setCurve(93,519,53,502,52,475,64,460);
        CubicCurve2D iHair4 = new CubicCurve2D.Double();
        iHair4.setCurve(96,517,60,497,58,474,64,460);
        CubicCurve2D bHair4 = new CubicCurve2D.Double();
        bHair4.setCurve(107,523,80,517,88,500,80,484);
        CubicCurve2D bHair4b = new CubicCurve2D.Double();
        bHair4b.setCurve(80,484,60,474,70,460,84,445);
        
        CubicCurve2D oHair5 = new CubicCurve2D.Double();
        oHair5.setCurve(63,474,26,463,9,408,42,375);
        CubicCurve2D iHair5 = new CubicCurve2D.Double();
        iHair5.setCurve(63,474,12,426,28,387,45,378);
        CubicCurve2D bHair5 = new CubicCurve2D.Double();
        bHair5.setCurve(84,445,95,475,33,463,39,429);
        CubicCurve2D bHair5b = new CubicCurve2D.Double();
        bHair5b.setCurve(39,429,28,406,41,390,61,384);
        
        
        
        CubicCurve2D oHair6 = new CubicCurve2D.Double();
        oHair6.setCurve(44,378,10,356,13,319,45,282);
        CubicCurve2D iHair6 = new CubicCurve2D.Double();
        iHair6.setCurve(45,378,23,351,23,314,45,282);
        CubicCurve2D bHair6 = new CubicCurve2D.Double();
        bHair6.setCurve(61,384,33,361,37,318,61,293);
        
        CubicCurve2D oHair7 = new CubicCurve2D.Double();
        oHair7.setCurve(49,285,46,268,62,249,84,249);
        CubicCurve2D iHair7 = new CubicCurve2D.Double();
        iHair7.setCurve(49,285,51,271,64,253,84,249);
        CubicCurve2D bHair7 = new CubicCurve2D.Double();
        bHair7.setCurve(61,293,64,276,84,263,104,262);
        
        CubicCurve2D oHair8 = new CubicCurve2D.Double();
        oHair8.setCurve(84,249,80,209,102,185,122,183);
        CubicCurve2D iHair8 = new CubicCurve2D.Double();
        iHair8.setCurve(84,249,87,211,109,190,121,186);
        CubicCurve2D bHair8 = new CubicCurve2D.Double();
        bHair8.setCurve(104,262,90,230,110,201,149,202);
        
        CubicCurve2D oHair9 = new CubicCurve2D.Double();
        oHair9.setCurve(120,186,112,174,128,149,153,142);
        CubicCurve2D iHair9 = new CubicCurve2D.Double();
        iHair9.setCurve(123,183,117,175,128,154,150,147);
        CubicCurve2D bHair9 = new CubicCurve2D.Double();
        bHair9.setCurve(149,202,134,185,146,164,167,156);
        
        CubicCurve2D oHair10 = new CubicCurve2D.Double();
        oHair10.setCurve(150,147,156,115,222,84,261,102);
        CubicCurve2D iHair10 = new CubicCurve2D.Double();
        iHair10.setCurve(159,147,161,114,220,88,253,109);
        CubicCurve2D bHair10 = new CubicCurve2D.Double();
        bHair10.setCurve(167,156,166,131,218,115,252,130);
        
        CubicCurve2D oHair11 = new CubicCurve2D.Double();
        oHair11.setCurve(250,107,255,97,272,92,279,94);
        CubicCurve2D iHair11 = new CubicCurve2D.Double();
        iHair11.setCurve(253,107,261,102,269,98,279,94);
        CubicCurve2D bHair11 = new CubicCurve2D.Double();
        bHair11.setCurve(252,130,261,120,274,112,288,112);
        
        CubicCurve2D oHair12 = new CubicCurve2D.Double();
        oHair12.setCurve(269,98,270,57,377,21,419,67);
        CubicCurve2D iHair12 = new CubicCurve2D.Double();
        iHair12.setCurve(279,98,277,56,375,32,414,69);
        CubicCurve2D bHair12 = new CubicCurve2D.Double();
        bHair12.setCurve(288,112,284,60,353,40,406,86);
        
        CubicCurve2D oHair13 = new CubicCurve2D.Double();
        oHair13.setCurve(409,76,417,53,453,50,465,57);
        CubicCurve2D iHair13 = new CubicCurve2D.Double();
        iHair13.setCurve(409,76,419,61,449,56,461,61);
        CubicCurve2D bHair13 = new CubicCurve2D.Double();
        bHair13.setCurve(406,86,421,75,445,65,462,72);
        
        CubicCurve2D oHair14 = new CubicCurve2D.Double();
        oHair14.setCurve(455,56,459,23,531,17,565,90);
        CubicCurve2D iHair14 = new CubicCurve2D.Double();
        iHair14.setCurve(464,59,467,27,525,21,554,89);
        CubicCurve2D bHair14 = new CubicCurve2D.Double();
        bHair14.setCurve(462,72,483,33,541,44,551,94);
        
        CubicCurve2D oHair15 = new CubicCurve2D.Double();
        oHair15.setCurve(554,87,581,62,638,105,643,149);
        CubicCurve2D iHair15 = new CubicCurve2D.Double();
        iHair15.setCurve(561,95,582,72,627,108,635,144);
        CubicCurve2D bHair15 = new CubicCurve2D.Double();
        bHair15.setCurve(551,94,584,89,619,129,613,163);
        
        CubicCurve2D oHair16 = new CubicCurve2D.Double();
        oHair16.setCurve(624,144,670,139,711,177,705,220);
        CubicCurve2D iHair16 = new CubicCurve2D.Double();
        iHair16.setCurve(626,150,668,144,705,189,696,218);
        CubicCurve2D bHair16 = new CubicCurve2D.Double();
        bHair16.setCurve(613,163,656,160,693,191,688,224);

        CubicCurve2D oHair17 = new CubicCurve2D.Double();
        oHair17.setCurve(699,215,718,212,743,228,743,246);
        CubicCurve2D iHair17 = new CubicCurve2D.Double();
        iHair17.setCurve(701,220,720,219,738,231,738,245);
        CubicCurve2D bHair17 = new CubicCurve2D.Double();
        bHair17.setCurve(688,224,707,223,726,235,730,251);
        
        CubicCurve2D oHair18 = new CubicCurve2D.Double();
        oHair18.setCurve(739,244,770,236,805,293,773,355);
        CubicCurve2D iHair18 = new CubicCurve2D.Double();
        iHair18.setCurve(740,249,763,240,796,300,773,351);
        CubicCurve2D bHair18 = new CubicCurve2D.Double();
        bHair18.setCurve(730,251,751,250,768,259,775,282);
        CubicCurve2D bHair18b = new CubicCurve2D.Double();
        bHair18b.setCurve(775,282,784,300,782,336,773,351);
        
        CubicCurve2D oHair19 = new CubicCurve2D.Double();
        oHair19.setCurve(755,338,804,360,780,419,747,442);
        CubicCurve2D iHair19 = new CubicCurve2D.Double();
        iHair19.setCurve(755,341,787,366,774,419,743,437);
        CubicCurve2D bHair19 = new CubicCurve2D.Double();
        bHair19.setCurve(731,331,787,366,785,423,730,442);
        CubicCurve2D wHair19 = new CubicCurve2D.Double();
        wHair19.setCurve(731,331,739,332,748,335,755,338);
        
        CubicCurve2D oHair20 = new CubicCurve2D.Double();
        oHair20.setCurve(747,435,771,451,765,484,740,500);
        CubicCurve2D iHair20 = new CubicCurve2D.Double();
        iHair20.setCurve(745,442,766,452,760,483,740,500);
        CubicCurve2D bHair20 = new CubicCurve2D.Double();
        bHair20.setCurve(730,442,751,452,745,483,724,500);
        
        CubicCurve2D oHair21 = new CubicCurve2D.Double();
        oHair21.setCurve(741,497,778,528,771,576,735,602);
        CubicCurve2D iHair21 = new CubicCurve2D.Double();
        iHair21.setCurve(740,500,760,516,762,570,735,590);
        CubicCurve2D bHair21 = new CubicCurve2D.Double();
        bHair21.setCurve(724,500,751,517,769,566,735,590);
        
        CubicCurve2D oHair22 = new CubicCurve2D.Double();
        oHair22.setCurve(721,575,780,621,704,660,683,649);
        CubicCurve2D iHair22 = new CubicCurve2D.Double();
        iHair22.setCurve(718,578,758,617,699,651,685,646);
        CubicCurve2D bHair22 = new CubicCurve2D.Double();
        bHair22.setCurve(718,578,758,617,699,651,685,646);
        
        CubicCurve2D oHair23 = new CubicCurve2D.Double();
        oHair23.setCurve(685,647,681,670,626,686,602,670);
        CubicCurve2D iHair23 = new CubicCurve2D.Double();
        iHair23.setCurve(681,648,674,668,632,676,615,666);
        CubicCurve2D bHair23 = new CubicCurve2D.Double();
        bHair23.setCurve(681,648,674,668,632,676,615,666);
        
// inner hair
        
        CubicCurve2D oHair24 = new CubicCurve2D.Double();
        oHair24.setCurve(615,457,599,453,595,438,609,429);
        CubicCurve2D iHair24 = new CubicCurve2D.Double();
        iHair24.setCurve(615,454,602,450,600,438,610,432);
        CubicCurve2D bHair24 = new CubicCurve2D.Double();
        bHair24.setCurve(617,454,607,448,605,439,613,432);
        
        CubicCurve2D oHair25 = new CubicCurve2D.Double();
        oHair25.setCurve(610,432,590,423,594,395,618,379);
        CubicCurve2D iHair25 = new CubicCurve2D.Double();
        iHair25.setCurve(611,429,594,418,597,399,618,379);
        CubicCurve2D bHair25 = new CubicCurve2D.Double();
        bHair25.setCurve(611,429,602,415,602,392,618,379);
        
        CubicCurve2D oHair26 = new CubicCurve2D.Double();
        oHair26.setCurve(620,379,595,382,584,356,591,337);
        CubicCurve2D bHair26 = new CubicCurve2D.Double();
        bHair26.setCurve(618,375,598,374,588,353,591,337);
        
        CubicCurve2D oHair27 = new CubicCurve2D.Double();
        oHair27.setCurve(600,340,588,341,575,330,577,316);
        CubicCurve2D bHair27 = new CubicCurve2D.Double();
        bHair27.setCurve(595,335,587,338,579,328,580,318);
        
        CubicCurve2D oHair28 = new CubicCurve2D.Double();
        oHair28.setCurve(585,334,559,340,538,328,534,307);
        CubicCurve2D bHair28 = new CubicCurve2D.Double();
        bHair28.setCurve(577,330,555,332,539,320,538,307);
       
        CubicCurve2D oHair29 = new CubicCurve2D.Double();
        oHair29.setCurve(537,308,519,318,492,306,486,283);
        CubicCurve2D bHair29 = new CubicCurve2D.Double();
        bHair29.setCurve(540,306,516,311,495,301,486,283);

        CubicCurve2D oHair30 = new CubicCurve2D.Double();
        oHair30.setCurve(493,300,479,304,468,297,465,286);
        CubicCurve2D bHair30 = new CubicCurve2D.Double();
        bHair30.setCurve(489,297,479,299,471,294,465,286);
        
        CubicCurve2D oHair31 = new CubicCurve2D.Double();
        oHair31.setCurve(465,286,452,310,411,313,397,286);
        CubicCurve2D bHair31 = new CubicCurve2D.Double();
        bHair31.setCurve(465,286,450,303,413,306,397,286);
        
        CubicCurve2D oHair32 = new CubicCurve2D.Double();
        oHair32.setCurve(397,286,382,305,358,310,339,292);
        CubicCurve2D bHair32 = new CubicCurve2D.Double();
        bHair32.setCurve(397,286,381,298,359,302,339,292);
        //
        CubicCurve2D oHair33 = new CubicCurve2D.Double();
        oHair33.setCurve(349,301,342,326,311,338,291,324);
        CubicCurve2D bHair33 = new CubicCurve2D.Double();
        bHair33.setCurve(347,300,336,322,313,330,291,324);
        
        CubicCurve2D oHair34 = new CubicCurve2D.Double();
        oHair34.setCurve(291,324,286,342,266,345,251,335);
        CubicCurve2D bHair34 = new CubicCurve2D.Double();
        bHair34.setCurve(291,324,280,338,265,338,251,335);
        
        CubicCurve2D oHair35 = new CubicCurve2D.Double();
        oHair35.setCurve(251,335,251,353,234,372,207,366);
        CubicCurve2D bHair35 = new CubicCurve2D.Double();
        bHair35.setCurve(251,335,245,351,226,366,207,366);
        
        CubicCurve2D oHair36 = new CubicCurve2D.Double();
        oHair36.setCurve(207,366,227,383,230,398,219,411);
        CubicCurve2D iHair36 = new CubicCurve2D.Double();
        iHair36.setCurve(207,366,222,383,221,399,212,413);
        CubicCurve2D bHair36 = new CubicCurve2D.Double();
        bHair36.setCurve(207,366,215,383,216,400,207,412);
        //
        CubicCurve2D oHair37 = new CubicCurve2D.Double();
        oHair37.setCurve(218,406,229,414,227,431,216,439);
        CubicCurve2D iHair37 = new CubicCurve2D.Double();
        iHair37.setCurve(213,406,223,415,221,430,216,439);
        CubicCurve2D bHair37 = new CubicCurve2D.Double();
        bHair37.setCurve(207,412,218,418,218,434,208,440);
                
        int[] x = { 237,147,132,107,80,84,39,61,61,104,149,
            167,252,288,406,462,551,613,688,730,775,773,755,731,730,
            724,735,721,719,685,681,615,613,/*inner hair*/611,
            618,618,591,600,595,580,538,536,486,465,397,339,
            291,251,207,207,208,
            221,
            };
        int[] y = { 632,647,600,523,484,445,429,384,293,262,202,
            156,130,112,86,72,94,163,224,251,282,351,338,331,442,
            500,590,575,579,648,649,666,454,/*inner hair*/429,
            379,375,354,340,335,318,307,305,283,286,286,292,
            324,335,366,412,440,
            497,
            };
        //
        CubicCurve2D oHair38 = new CubicCurve2D.Double();
        oHair38.setCurve(216,436,229,436,240,448,238,468);
        CubicCurve2D iHair38 = new CubicCurve2D.Double();
        iHair38.setCurve(214,437,228,442,234,456,236,466);
        CubicCurve2D bHair38 = new CubicCurve2D.Double();
        bHair38.setCurve(208,440,223,451,228,474,221,497);
        
        int[] p1x = {592,588,606};
        int[] p1y = {336,355,340};
        Polygon patch1 = new Polygon(p1x, p1y, 3);
         //585,334,  536,305,    535,280,  614,314
        int[] p2x = {585,533,535,614};
        int[] p2y = {335,308,280,314};
        Polygon patch2 = new Polygon(p2x, p2y, 4);
        //493,300,  465,286,   459,266   516,286
        int[]p3x = {493,465,459,516};
        int[]p3y = {300,286,266,286};
        Polygon patch3 = new Polygon(p3x, p3y, 4);
         // 347,302   291,324    272, 303    368,270       
        int[] p4x = {347,291,272,368};
        int[] p4y = {302,324,303,270};
        Polygon patch4 = new Polygon(p4x, p4y, 4);
        
        
        G.setColor(new Color(0,173,239));
        G.fill(patch1);
        G.fill(patch2);
        G.fill(patch3);
        G.fill(patch4);
        
        G.setColor(Color.BLACK);
        G.fill(oHair1);
        G.fill(oHair2);
        G.fill(oHair3);
        G.fill(oHair5);
        G.setColor(Color.WHITE);
        G.fill(iHair1);
        G.fill(iHair2);
        G.fill(iHair3);
        G.fill(iHair5);
        G.setColor(Color.BLACK);
        G.fill(oHair4);
        G.setColor(Color.WHITE);
        G.fill(iHair4);
        
        
        G.setColor(Color.BLACK);
        G.fill(oHair6);
        G.fill(oHair7);
        G.fill(oHair8);
        G.fill(oHair9);
        G.fill(oHair10);
        G.fill(oHair11);
        G.fill(oHair12);
        G.fill(oHair13);
        G.fill(oHair14);
        G.fill(oHair15);
        G.fill(oHair16);
        G.fill(oHair17);
        G.fill(oHair18);
        G.fill(oHair19);
        G.fill(oHair20);
        G.fill(oHair21);
        G.fill(oHair22);
        G.fill(oHair23);
        

        G.setColor(Color.WHITE);
        G.fill(iHair6);
        G.fill(iHair7);
        G.fill(iHair8);
        G.fill(iHair9);
        G.fill(iHair10);
        G.fill(iHair11);
        G.fill(iHair12);
        G.fill(iHair13);
        G.fill(iHair14);
        G.fill(iHair15);
        G.fill(iHair16);
        G.fill(iHair17);
        G.fill(iHair18);
        G.fill(iHair19);
        G.fill(iHair20);
        G.fill(iHair21);
        G.fill(iHair22);
        G.fill(iHair23);
        //inner hair
        
        
        
        G.setColor(new Color(0,173,239));
        G.fill(bHair1);
        G.fill(bHair2);
        G.fill(bHair3);
        G.fill(bHair4);
        G.fill(bHair4b);
        G.fill(bHair5);
        G.fill(bHair5b);
        G.fill(bHair6);
        G.fill(bHair7);
        G.fill(bHair8);
        G.fill(bHair9);
        G.fill(bHair10);
        G.fill(bHair11);
        G.fill(bHair12);
        G.fill(bHair13);
        G.fill(bHair14);
        G.fill(bHair15);
        G.fill(bHair16);
        G.fill(bHair17);
        G.fill(bHair18);
        G.fill(bHair18b);
        G.fill(bHair19);
        G.fill(bHair20);
        G.fill(bHair21);
        G.fill(bHair22);
        G.fill(bHair23);
        
        
        //inner hair
        G.setColor(Color.BLACK);
        G.fill(oHair24);
        G.fill(oHair25);
        G.fill(oHair26);
        G.setColor(Color.WHITE);
        G.fill(iHair24);
        G.fill(iHair25);
        G.setColor(new Color(0,173,239));
        G.fill(bHair24);
        G.fill(bHair25);
        G.fill(bHair26);// iHair 26-35 n/a
        G.setColor(Color.BLACK);
        G.fill(oHair27);
        G.setColor(new Color(0,173,239));
        G.fill(bHair27);
        G.setColor(Color.BLACK);
        G.fill(oHair28);
        G.setColor(new Color(0,173,239));
        G.fill(bHair28);
        G.setColor(Color.BLACK);
        G.fill(oHair29);
        G.setColor(new Color(0,173,239));
        G.fill(bHair29);
        G.setColor(Color.BLACK);
        G.fill(oHair30);
        G.setColor(new Color(0,173,239));
        G.fill(bHair30);
        G.setColor(Color.BLACK);
        G.fill(oHair31);
        G.setColor(new Color(0,173,239));
        G.fill(bHair31);
        G.setColor(Color.BLACK);
        G.fill(oHair32);
        G.setColor(new Color(0,173,239));
        G.fill(bHair32);
        G.setColor(Color.BLACK);
        G.fill(oHair33);
        G.setColor(new Color(0,173,239));
        G.fill(bHair33);
        G.setColor(Color.BLACK);
        G.fill(oHair34);
        G.setColor(new Color(0,173,239));
        G.fill(bHair34);
        G.setColor(Color.BLACK);
        G.fill(oHair35);
        G.setColor(new Color(0,173,239));
        G.fill(bHair35);
        G.setColor(Color.BLACK);
        G.fill(oHair36);
        G.setColor(Color.WHITE);
        G.fill(iHair36);
        G.setColor(new Color(0,173,239));
        G.fill(bHair36);
        G.setColor(Color.BLACK);
        G.fill(oHair37); 
        G.setColor(Color.WHITE);
        G.fill(iHair37);
        G.setColor(new Color(0,173,239));
        G.fill(bHair37);
        G.setColor(Color.BLACK);
        G.fill(oHair38);
        G.setColor(Color.WHITE);
        G.fill(iHair38);
        G.setColor(new Color(0,173,239));
        G.fill(bHair38);
        

        
        
        
        
        
        
        
        

        
        Polygon hairFill = new Polygon(x, y, 52);
        
        G.fill(hairFill);
        
        G.setColor(Color.WHITE);
        G.fill(wHair19);
        
        //237,446  204,630   274,752   411,803
        CubicCurve2D oLFace = new CubicCurve2D.Double();
        oLFace.setCurve(233,467,140,630,194,752,411,803);
        G.setColor(Color.BLACK);
        G.fill(oLFace);
        
        G.setColor(Color.WHITE);
        CubicCurve2D iLFace = new CubicCurve2D.Double();
        iLFace.setCurve(233,467,159,630,222,752,411,803);
        G.fill(iLFace);
        // 595,449   411,803
        
        G.setColor(Color.BLACK);
        CubicCurve2D oRFace = new CubicCurve2D.Double();
        oRFace.setCurve(595,449,688,630,636,752,411,803);
        G.fill(oRFace);
        
        G.setColor(Color.WHITE);
        CubicCurve2D iRFace = new CubicCurve2D.Double();
        iRFace.setCurve(595,449,669,630,606,752,411,803);
        G.fill(iRFace);
        
        G.setColor(new Color(244,170,145)); // tan color
        CubicCurve2D tanChin = new CubicCurve2D.Double();
        tanChin.setCurve(293,753,400,813,422,813,527,756);
        G.fill(tanChin);
        
        
        G.setColor(Color.BLACK);
        CubicCurve2D blChin = new CubicCurve2D.Double();
        blChin.setCurve(335,744,381,823,443,823,496,744);
        G.fill(blChin);
        
        G.setColor(Color.WHITE);
        CubicCurve2D wChin = new CubicCurve2D.Double();
        wChin.setCurve(335,744,385,811,434,811,496,744);
        G.fill(wChin);
        
     
        
        G.setColor(new Color(244,170,145)); // tan color
        G.rotate(-.26);
        G.fillOval(170,428,100,50);
        
        G.setColor(new Color(0,173,239)); // blue color
        CubicCurve2D bLBrow = new CubicCurve2D.Double();
        bLBrow.setCurve(170,450,180,421,268,415,271,455);
        G.fill(bLBrow);
        
        G.setColor(new Color(244,170,145)); // tan color
        G.rotate(.36);
        G.fillOval(515,310,80,35);
        
        G.setColor(new Color(0,173,239)); // blue color
        G.rotate(.16);
        G.fillOval(550,205,100,35);
        
        G.rotate(-.26); // back to normal rotation
        
        //eyes
        G.setColor(Color.BLACK);
        G.fillOval(262, 405, 90, 90);
        G.setColor(Color.WHITE);
        G.fillOval(266,409, 78, 78);
        G.setColor(Color.BLACK);
        G.fillOval(292,431,32,32);
        G.setColor(Color.WHITE);
        G.fillOval(298,433,20,20);
        G.setColor(Color.BLACK);
        CubicCurve2D lEyePatch = new CubicCurve2D.Double();
        lEyePatch.setCurve(296,452,305,460,314,460,316,449);
        G.fill(lEyePatch);
        
        G.setColor(Color.BLACK);
        G.fillOval(476,402,90,92);
        G.setColor(Color.WHITE);
        G.fillOval(482,406,78,78);
        G.setColor(Color.BLACK);
        G.fillOval(505,427,32,32);
        G.setColor(Color.WHITE);
        G.fillOval(511,429,20,20);
        G.setColor(Color.BLACK);
        CubicCurve2D rEyePatch = new CubicCurve2D.Double();
        rEyePatch.setCurve(509,448,518,456,526,456,534,440);
        G.fill(rEyePatch);
        
        G.setStroke(new BasicStroke(3));
        CubicCurve2D blLBrowTop = new CubicCurve2D.Double();
        blLBrowTop.setCurve(278,391,275,349,357,329,382,367);
        G.draw(blLBrowTop);
        CubicCurve2D lEyeB1 = new CubicCurve2D.Double();
        lEyeB1.setCurve(278,391,294,380,305,378,330,380);
        G.fill(lEyeB1);
        CubicCurve2D lEyeB2 = new CubicCurve2D.Double();
        lEyeB2.setCurve(284,388,324,392,352,395,386,367);
        G.fill(lEyeB2);
        G.setStroke(new BasicStroke(7));
        CubicCurve2D lEyeB3 = new CubicCurve2D.Double();
        lEyeB3.setCurve(333,384,354,378,369,371,378,368);
        G.draw(lEyeB3);
        G.setStroke(new BasicStroke(2));
        CubicCurve2D lEyeB4 = new CubicCurve2D.Double();
        lEyeB4.setCurve(296,383,296,381,297,374,300,367);
        G.draw(lEyeB4);
        int[] lEyeX5 = {308,313,315};
        int[] lEyeY5 = {384,384,350};
        Polygon lEyeB5 = new Polygon(lEyeX5,lEyeY5,3);
        G.setStroke(new BasicStroke(2));
        G.fill(lEyeB5);
        CubicCurve2D lEyeB6 = new CubicCurve2D.Double();
        lEyeB6.setCurve(324,387,323,375,326,365,328,353);
        G.draw(lEyeB6);
        CubicCurve2D lEyeB7 = new CubicCurve2D.Double();
        lEyeB7.setCurve(326,387,325,375,328,365,328,353);
        G.draw(lEyeB7);
        CubicCurve2D lEyeB8 = new CubicCurve2D.Double();
        lEyeB8.setCurve(340,387,339,375,342,365,343,352);
        G.draw(lEyeB8);
        lEyeB8.setCurve(342,387,341,375,344,365,343,352);
        G.draw(lEyeB8);
        lEyeB8.setCurve(356,380,357,375,357,365,358,354);
        G.draw(lEyeB8);
        lEyeB8.setCurve(358,380,359,375,359,365,358,354);
        G.draw(lEyeB8);
        
        /// right eyebrow
        G.rotate(.40);
        G.translate(296, -234);
        G.setStroke(new BasicStroke(3));
        blLBrowTop.setCurve(278,391,275,349,357,329,382,367);
        G.draw(blLBrowTop);
        lEyeB1.setCurve(278,391,294,380,305,378,330,380);
        G.fill(lEyeB1);
        lEyeB2.setCurve(284,388,324,392,352,395,386,367);
        G.fill(lEyeB2);
        G.setStroke(new BasicStroke(7));
        lEyeB3.setCurve(333,384,354,378,369,371,378,368);
        G.draw(lEyeB3);
        G.setStroke(new BasicStroke(2));
        lEyeB4.setCurve(296,383,296,381,297,374,300,367);
        G.draw(lEyeB4);
        int[] lEyeX5b = {308,313,315};
        int[] lEyeY5b = {384,384,350};
        Polygon lEyeB5b = new Polygon(lEyeX5b,lEyeY5b,3);
        G.setStroke(new BasicStroke(2));
        G.fill(lEyeB5b);
        lEyeB6.setCurve(324,387,323,375,326,365,328,353);
        G.draw(lEyeB6);
        lEyeB7.setCurve(326,387,325,375,328,365,328,353);
        G.draw(lEyeB7);
        lEyeB8.setCurve(340,387,339,375,342,365,343,352);
        G.draw(lEyeB8);
        lEyeB8.setCurve(342,387,341,375,344,365,343,352);
        G.draw(lEyeB8);
        lEyeB8.setCurve(356,380,357,375,357,365,358,354);
        G.draw(lEyeB8);
        lEyeB8.setCurve(358,380,359,375,359,365,358,354);
        G.draw(lEyeB8);
        
        
        G.translate(-296, 234);
        G.rotate(-.40); // back to normal rotation
        
        //nose
        G.fillOval(355,458,103,114);
        G.setColor(Color.WHITE);
        G.fillOval(360,461,93,99);
        G.setColor(new Color(237,27,36)); //Red color
        G.fillOval(365,475,86,86);
        G.setColor(new Color(157,7,8)); // dark red color
        CubicCurve2D noseShade = new CubicCurve2D.Double();
        noseShade.setCurve(372,518,370,560,410,582,443,529);
        G.fill(noseShade);
        G.setColor(new Color(237,27,36)); //red color
        G.fillOval(372,500,75,45);
        
        //mouth
        G.fillOval(208,550,150,154);
        G.fillOval(464,550,150,154);
        
        G.setStroke(new BasicStroke(7));
        CubicCurve2D upperMouthRed1 = new CubicCurve2D.Double();
        upperMouthRed1.setCurve(308,559,346,580,361,593,463,595);
        G.draw(upperMouthRed1);
        CubicCurve2D upperMouthRed2 = new CubicCurve2D.Double();
        upperMouthRed2.setCurve(514,559,476,580,461,594,359,595);
        G.draw(upperMouthRed2);
        
        int[] xMouth1 = { 308,346,361,410,460,476,514,539,283 };
        int[] yMouth1 = { 562,578,586,595,585,576,562,704,704 };
        Polygon p = new Polygon(xMouth1, yMouth1,9);
        G.fill(p);
        
        CubicCurve2D lowerMouth = new CubicCurve2D.Double();
        lowerMouth.setCurve(239,690,361,769,469,769,583,690);
        G.fill(lowerMouth);
        
        // 262,627,,,,,,546,655
        G.setStroke(new BasicStroke(3));
        
        //line around mouth
        G.setColor(Color.BLACK);
        CubicCurve2D line = new CubicCurve2D.Double();
        line.setCurve(205,645,320,785,530,781,620,648);
        G.draw(line);
        line.setCurve(205,655,185,528,273,513,329,555);
        G.draw(line);
        line.setCurve(329,555,377,593,422,588,489,560);
        G.draw(line);
        line.setCurve(489,560,577,511,620,574,624,611);
        G.draw(line);
        
        //inner mouth
        int[] inMoX = { 302,330,361,405,450,473,496,558,546,262 };
        int[] inMoY = { 610,620,629,633,628,621,611,639,665,637 };
        Polygon innerMouthP = new Polygon(inMoX, inMoY, 10);
        G.setColor(new Color(157,7,8)); // dark red
        G.fill(innerMouthP);
        
        
        line.setCurve(262,637,308,693,424,716,546,665);
        G.setColor(new Color(157,7,8)); // dark red
        G.fill(line); //bottom inner
        G.setColor(Color.BLACK);
        G.draw(line); // bottom inner
        
        line.setCurve(302,610,361,641,450,640,496,612);
        G.draw(line); // top inner
        
        G.setColor(new Color(157,7,8)); // dark red
        line.setCurve(496,611,526,605,556,623,558,639);
        G.fill(line); // right top inner
        G.setColor(Color.BLACK);
        G.draw(line);
        line.setCurve(496,611,523,605,553,623,555,639);
        G.draw(line);
        line.setCurve(496,611,520,605,550,623,552,639);
        G.draw(line);
        
        G.setColor(new Color(157,7,8));
        line.setCurve(558,639,559,646,559,657,546,665);
        G.fill(line); // right bottom inner
        G.setColor(Color.BLACK);
        G.draw(line);
        line.setCurve(555,639,556,646,556,657,546,665);
        G.draw(line);
        line.setCurve(552,639,553,646,553,657,546,665);
        G.draw(line);
                
        G.setColor(new Color(157,7,8)); // dark red
        line.setCurve(262,637,250,619,276,594,302,610);
        G.fill(line); // left inner
        G.setColor(Color.BLACK);
        G.draw(line);
        line.setCurve(262,637,253,619,279,594,302,610);
        G.draw(line);
         
        //inside mouth tongue etc.
        G.fillOval(371,646,66,30);
        G.setColor(new Color(157,7,8));
        G.fillOval(388,644,33,20);
        G.setColor(Color.BLACK);
        line.setCurve(348,684,346,666,376,669,403,674);
        G.draw(line);
        line.setCurve(458,688,464,666,430,669,403,674);
        G.draw(line);
        //teeth
        G.setColor(Color.BLACK);
        line.setCurve(301,609,338,677,469,677,502,609);
        G.fill(line);
        G.setColor(Color.WHITE);
        line.setCurve(304,609,338,674,469,674,499,611);
        G.fill(line);
        
        
        int[] bt1X = {303,335,330};
        int[] bt1Y = {609,613,638};
        Polygon blueTeeth1 = new Polygon(bt1X, bt1Y,3);
        G.setColor(new Color(147,217,251)); // light blue color
        G.fill(blueTeeth1);
        line.setCurve(466,627,474,637,482,637,497,612);
        G.fill(line);
        
        line.setCurve(300,608,361,641,450,642,500,609);
        G.setColor(new Color(237,27,36)); // red color
        G.fill(line);
        line.setCurve(302,610,361,641,450,640,497,612);
        G.setColor(Color.BLACK);
        G.draw(line); // top inner
        line.setCurve(302,610,361,644,450,643,497,612);
        G.draw(line);
        line.setCurve(302,610,361,647,450,646,497,612);
        G.draw(line);
        
        //tooth lines
        line.setCurve(321,618,321,620,321,621,321,630);
        G.draw(line);
        line.setCurve(341,628,341,629,341,631,341,645);
        G.draw(line);
        line.setCurve(365,633,363,636,364,642,365,653);
        G.draw(line);
        line.setCurve(403,635,403,636,402,637,402,658);
        G.draw(line);
        line.setCurve(440,633,438,638,437,649,439,653);
        G.draw(line);
        line.setCurve(465,628,466,633,467,639,467,642);
        G.draw(line);
        line.setCurve(489,618,489,620,490,621,490,623);
        G.draw(line);
        
        // dimples around mouth
        line.setCurve(234,634,216,601,232,574,270,569); // left dimple
        G.setColor(new Color(157,7,8)); // dark red
        G.fill(line);
        G.setColor(Color.BLACK);
        line.setCurve(234,634,230,601,246,574,282,564);
        G.fill(line);
        G.setColor(new Color(237,27,36)); // red color
        line.setCurve(234,634,237,601,257,574,282,564);
        G.fill(line);
        G.setStroke(new BasicStroke(2));
        G.setColor(Color.BLACK); // detail lines
        line.setCurve(233,605,235,608,237,610,240,613);
        G.draw(line);
        G.translate(5,-10);
        G.draw(line);
        G.translate(5,-10);
        G.draw(line);
        G.translate(8,-5);
        line.setCurve(233,605,233,605,233,605,236,611);
        G.draw(line);
        G.translate(-18,25); //back to normal
        
        
        line.setCurve(537,572,595,588,601,609,579,622); // right dimple
        G.setColor(new Color(157,7,8)); // dark red
        G.fill(line);
        G.setColor(Color.BLACK);
        line.setCurve(537,572,565,578,587,609,579,632);
        G.fill(line);
        G.setColor(new Color(237,27,36)); // red color
        line.setCurve(537,572,559,578,575,609,576,632);
        G.fill(line);
        G.setStroke(new BasicStroke(3));
        G.setColor(Color.BLACK); // detail lines
        line.setCurve(563,587,563,587,563,587,566,583);
        G.draw(line);
        G.translate(5,5);
        line.setCurve(563,587,563,587,563,587,570,579);
        G.draw(line);
        G.translate(4,8);
        line.setCurve(559,590,559,590,559,590,569,580);
        G.draw(line);
        G.translate(-9,-13); // back to normal
        line.setCurve(574,608,574,608,574,608,582,603);
        G.draw(line);
        line.setCurve(576,613,576,613,576,613,586,608);
        G.draw(line);
        
        G.setColor(new Color(157,7,8)); // bottom mouth dimple
        line.setCurve(364,719,401,746,429,740,440,719);
        G.fill(line);
        G.setColor(Color.BLACK);
        line.setCurve(343,708,390,739,440,728,467,708);
        G.fill(line);
        G.setColor(new Color(237,27,36));// red color
        line.setCurve(343,706,390,732,440,721,467,706);
        G.fill(line);
        G.setStroke(new BasicStroke(2));
        G.setColor(Color.BLACK);
        line.setCurve(385,723,385,723,385,723,398,732);
        G.draw(line);
        line.setCurve(401,725,401,725,401,725,409,733);
        G.draw(line);
        line.setCurve(411,722,411,722,411,722,423,734);
        G.draw(line);
        line.setCurve(428,721,428,721,428,721,432,728);
        G.draw(line);
        Font font = new Font("Courier", Font.BOLD,32);
        G.setFont(font);
        G.drawString("Hello, friend!", 350, 850);

        // CHANGE IMAGE SIZE TO MEET SIZE REQUIREMENTS
        
        BufferedImage newI = new BufferedImage(700,700,BufferedImage.TYPE_INT_RGB);
        Graphics2D G2 = newI.createGraphics();
        G2.fillRect(0,0,700,700);
        G2.translate(45,35); // CENTER IMAGE
        G2.drawImage(img,0,0,700,700,0,0,1000,1000,null);
        G2.dispose();
        // change to newI if wanting smaller image, but for large save with img!
        Image img2 = SwingFXUtils.toFXImage(newI, null);
        g.drawImage(img2, 0, 0);
        
    }
}
