/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package gateley_stones_groupproject_client;

import java.awt.Color;
import java.awt.Font;
import java.awt.event.MouseEvent;
import java.awt.event.MouseListener;
import javax.swing.ImageIcon;
import javax.swing.JFrame;
import javax.swing.JLabel;
import javax.swing.JPanel;
import javax.swing.JTextArea;
import static javax.swing.SwingConstants.CENTER;
import javax.swing.border.EmptyBorder;

/**
 *
 * @author Jeremy
 */
public class About extends JFrame {
    
    private JPanel contentPane;
    private JLabel title;
    private JLabel info1;
    private JLabel info2;
    private JLabel info3;
    private JLabel info4;
    private JLabel info5;
    private JLabel info6;
    private JLabel info7;
    private JLabel info8;
    private JLabel info9;
    private JLabel info10;
    private JLabel logo;
    
    public About () {
        setBackground(Color.GRAY);
        setDefaultCloseOperation(JFrame.DISPOSE_ON_CLOSE);
        setBounds(100, 100, 550, 672);
        contentPane = new JPanel();
        contentPane.setBackground(Color.BLACK);
        contentPane.setBorder(new EmptyBorder(5, 5, 5, 5));
        setContentPane(contentPane);
        contentPane.setLayout(null);        
        
        logo = new JLabel(new ImageIcon("logo.gif"));
        logo.setBounds(25,40,500,100);
        contentPane.add(logo);
        
        title = new JLabel();
        title.setText("Lazer Chat\u2122 Version 1.0");
        title.setFont(new Font("Kristen ITC", Font.BOLD, 18));
        title.setForeground(Color.WHITE);
        title.setBounds(150,200,300,25);
        contentPane.add(title);
        
        info1 = new JLabel();
        info1.setText("\u00A9 2016  Gateley Stones Industries");
        info1.setFont(new Font("Kristen ITC", Font.PLAIN, 13));
        info1.setForeground(Color.WHITE);
        info1.setBounds(150,250,300,15);
        info1.setHorizontalTextPosition(CENTER);
        info1.setAlignmentX(CENTER_ALIGNMENT);
        info1.setVerticalAlignment(CENTER);
        contentPane.add(info1);
        
        info2 = new JLabel();
        info2.setText("Patents Pending");
        info2.setFont(new Font("Kristen ITC", Font.PLAIN, 13));
        info2.setForeground(Color.WHITE);
        info2.setBounds(150,270,300,15);
        info2.setHorizontalTextPosition(CENTER);
        contentPane.add(info2);
        
        info3 = new JLabel();
        info3.setText("The Lazer Chat name, associated trademarks, and logos are");
        info3.setFont(new Font("Kristen ITC", Font.PLAIN, 13));
        info3.setForeground(Color.WHITE);
        info3.setBounds(100,290,400,15);
        contentPane.add(info3);
        
        info4 = new JLabel();
        info4.setText("trademarks of Gateley Stones Industries or related entities.");
        info4.setFont(new Font("Kristen ITC", Font.PLAIN, 13));
        info4.setForeground(Color.WHITE);
        info4.setBounds(100,310,400,15);
        contentPane.add(info4);
        
        info5 = new JLabel();
        info5.setText("Warning: This program is protected by copyright law");
        info5.setFont(new Font("Kristen ITC", Font.PLAIN, 13));
        info5.setForeground(Color.WHITE);
        info5.setBounds(100,330,400,15);
        contentPane.add(info5);
        
        info6 = new JLabel();
        info6.setText("and international treaties.");
        info6.setFont(new Font("Kristen ITC", Font.PLAIN, 13));
        info6.setForeground(Color.WHITE);
        info6.setBounds(100,350,400,15);
        contentPane.add(info6);
        
        info7 = new JLabel();
        info7.setText("Unauthorized reproduction or distribution of this program,");
        info7.setFont(new Font("Kristen ITC", Font.PLAIN, 13));
        info7.setForeground(Color.WHITE);
        info7.setBounds(100,370,400,15);
        contentPane.add(info7);
        
        info8 = new JLabel();
        info8.setText("or any portions of it, may result in severe civil");
        info8.setFont(new Font("Kristen ITC", Font.PLAIN, 13));
        info8.setForeground(Color.WHITE);
        info8.setBounds(150,390,310,15);
        contentPane.add(info8);
        
        info9 = new JLabel();
        info9.setText("and criminal penalties, and will be prosecuted");
        info9.setFont(new Font("Kristen ITC", Font.PLAIN, 13));
        info9.setForeground(Color.WHITE);
        info9.setBounds(150,410,310,15);
        contentPane.add(info9);
        
        info10 = new JLabel();
        info10.setText("to the maximum extent possible under the law.");
        info10.setFont(new Font("Kristen ITC", Font.PLAIN, 13));
        info10.setForeground(Color.WHITE);
        info10.setBounds(150,430,310,15);
        contentPane.add(info10);

        
        this.addMouseListener(new MouseListener() {
            @Override
            public void mouseClicked(MouseEvent e) {
                dispose();
             }

            @Override
            public void mousePressed(MouseEvent e) {
                throw new UnsupportedOperationException("Not supported yet."); //To change body of generated methods, choose Tools | Templates.
            }

            @Override
            public void mouseReleased(MouseEvent e) {
                dispose();
            }

            @Override
            public void mouseEntered(MouseEvent e) {
                throw new UnsupportedOperationException("Not supported yet."); //To change body of generated methods, choose Tools | Templates.
            }

            @Override
            public void mouseExited(MouseEvent e) {
                throw new UnsupportedOperationException("Not supported yet."); //To change body of generated methods, choose Tools | Templates.
            }
        });
        
    }
    
    
    
    
}
