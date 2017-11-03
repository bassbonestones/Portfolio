/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package gateley_stones_groupproject_client;

import java.awt.BorderLayout;
import java.awt.EventQueue;

import javax.swing.JFrame;
import javax.swing.JPanel;
import javax.swing.border.EmptyBorder;
import javax.swing.text.MaskFormatter;

import java.awt.Color;
import javax.swing.JTextField;
import javax.swing.JRadioButtonMenuItem;
import javax.swing.JRadioButton;
import javax.swing.ImageIcon;
import javax.swing.JFormattedTextField;
import javax.swing.JLabel;
import java.awt.Font;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.event.FocusEvent;
import java.awt.event.FocusListener;
import java.net.MalformedURLException;
import java.net.URL;
import java.text.NumberFormat;
import java.text.ParseException;
import javax.swing.ButtonGroup;
import javax.swing.JButton;

public class Login extends JFrame {

	private JPanel contentPane;
	private JTextField userField;
	private final ButtonGroup buttonGroup_IconType = new ButtonGroup();
	private final ButtonGroup buttonGroup_Faces = new ButtonGroup();
	private static String username;
	private static String IP;
	private static int port;
	private static boolean isURL;
	private static boolean isFunFace;
	private static int faceSelected;
	private static String imgURL;
	
	static Login frame = new Login();
	/**
	 * Launch the application.
	 */
	public static void main(String[] args) {
		EventQueue.invokeLater(new Runnable() {
			public void run() {
				try {
					alterFrame(frame);
					
				} catch (Exception e) {
					e.printStackTrace();
				}
			}
		});
	}
	public static void alterFrame(JFrame frame) {
		
		
	}
	/**
	 * Create the frame.
	 */
	public Login() {
		imgURL = null;
		faceSelected = 0;
		isURL = false;
		isFunFace = true;
		System.out.println(this.getParent());
		
		setBackground(Color.GRAY);
		setDefaultCloseOperation(JFrame.DISPOSE_ON_CLOSE);
		setBounds(100, 100, 519, 672);
		contentPane = new JPanel();
		contentPane.setBackground(Color.BLACK);
		contentPane.setBorder(new EmptyBorder(5, 5, 5, 5));
		setContentPane(contentPane);
		contentPane.setLayout(null);
		
		userField = new JTextField();
		userField.setForeground(Color.WHITE);
		userField.setBackground(Color.GRAY);
		userField.setBounds(42, 170, 211, 22);
		contentPane.add(userField);
		userField.setColumns(10);
		
		JRadioButton rdbtnImgURL = new JRadioButton("img URL");
		buttonGroup_IconType.add(rdbtnImgURL);
		rdbtnImgURL.setForeground(Color.WHITE);
		rdbtnImgURL.setBackground(Color.BLACK);
		rdbtnImgURL.setBounds(291, 274, 85, 25);
		contentPane.add(rdbtnImgURL);
		
		JRadioButton rdbtnFunFace = new JRadioButton("FunFace");
		buttonGroup_IconType.add(rdbtnFunFace);
		rdbtnFunFace.setForeground(Color.WHITE);
		rdbtnFunFace.setBackground(Color.BLACK);
		rdbtnFunFace.setSelected(true);
		rdbtnFunFace.setBounds(377, 274, 85, 25);
		contentPane.add(rdbtnFunFace);
		
		JTextField ipField = new JTextField();
		ipField.setForeground(Color.WHITE);
		ipField.setBackground(Color.GRAY);
		ipField.setBounds(291,170,160,22);
		contentPane.add(ipField);
			
		NumberFormat integerFieldFormatter = NumberFormat.getIntegerInstance();
		integerFieldFormatter.setGroupingUsed(false);
		JFormattedTextField portField = new JFormattedTextField(integerFieldFormatter);
		portField.setForeground(Color.WHITE);
		portField.setBackground(Color.GRAY);
		portField.setBounds(42, 277, 124, 22);
		contentPane.add(portField);
		
		JLabel lblUsername = new JLabel("Username:");
		lblUsername.setFont(new Font("Kristen ITC", Font.BOLD, 13));
		lblUsername.setForeground(Color.WHITE);
		lblUsername.setBackground(Color.GRAY);
		lblUsername.setBounds(43, 141, 91, 16);
		contentPane.add(lblUsername);
		
		JLabel lblHost = new JLabel("Host IP:");
		lblHost.setFont(new Font("Kristen ITC", Font.BOLD, 13));
		lblHost.setForeground(Color.WHITE);
		lblHost.setBackground(Color.GRAY);
		lblHost.setBounds(291, 141, 105, 16);
		contentPane.add(lblHost);
		
		JLabel lblHostEg = new JLabel("(eg. 192.68.0.2)");
		lblHostEg.setFont(new Font("Kristen ITC", Font.BOLD, 13));
		lblHostEg.setForeground(Color.WHITE);
		lblHostEg.setBackground(Color.GRAY);
		lblHostEg.setBounds(291, 205, 143, 16);
		contentPane.add(lblHostEg);
		
		JLabel lblPort = new JLabel("Port:");
		lblPort.setFont(new Font("Kristen ITC", Font.BOLD, 13));
		lblPort.setForeground(Color.WHITE);
		lblPort.setBackground(Color.GRAY);
		lblPort.setBounds(42, 248, 124, 16);
		contentPane.add(lblPort);
		
		JLabel lblPortEg = new JLabel("(eg. 55555)");
		lblPortEg.setFont(new Font("Kristen ITC", Font.BOLD, 13));
		lblPortEg.setForeground(Color.WHITE);
		lblPortEg.setBackground(Color.GRAY);
		lblPortEg.setBounds(42, 310, 354, 16);
		contentPane.add(lblPortEg);
		
		JLabel lblUserIcon = new JLabel("User Icon:");
		lblUserIcon.setFont(new Font("Kristen ITC", Font.BOLD, 13));
		lblUserIcon.setForeground(Color.WHITE);
		lblUserIcon.setBackground(Color.GRAY);
		lblUserIcon.setBounds(291, 248, 160, 16);
		contentPane.add(lblUserIcon);
		
		JPanel label = new JPanel();
		label.setBackground(Color.BLACK);
		label.setBounds(115, 3, 271, 116);
		label.setVisible(true);
		label.add(new JLabel(new ImageIcon("login.gif")));
		contentPane.add(label);
		
		JRadioButton rdbtnFace1 = new JRadioButton("");
		buttonGroup_Faces.add(rdbtnFace1);
		rdbtnFace1.setBackground(Color.BLACK);
		rdbtnFace1.setBounds(51, 348, 30, 25);
                rdbtnFace1.setSelected(true);
		contentPane.add(rdbtnFace1);
		
		JRadioButton rdbtnFace2 = new JRadioButton("");
		buttonGroup_Faces.add(rdbtnFace2);
		rdbtnFace2.setBackground(Color.BLACK);
		rdbtnFace2.setBounds(204, 348, 30, 25);
		contentPane.add(rdbtnFace2);
		
		JRadioButton rdbtnFace3 = new JRadioButton("");
		buttonGroup_Faces.add(rdbtnFace3);
		rdbtnFace3.setBackground(Color.BLACK);
		rdbtnFace3.setBounds(356, 348, 30, 25);
		contentPane.add(rdbtnFace3);
		
		JRadioButton rdbtnFace4 = new JRadioButton("");
		buttonGroup_Faces.add(rdbtnFace4);
		rdbtnFace4.setBackground(Color.BLACK);
		rdbtnFace4.setBounds(51, 442, 30, 25);
		contentPane.add(rdbtnFace4);
		
		JRadioButton rdbtnFace5 = new JRadioButton("");
		buttonGroup_Faces.add(rdbtnFace5);
		rdbtnFace5.setBackground(Color.BLACK);
		rdbtnFace5.setBounds(204, 442, 30, 25);
		contentPane.add(rdbtnFace5);
		
		JRadioButton rdbtnFace6 = new JRadioButton("");
		buttonGroup_Faces.add(rdbtnFace6);
		rdbtnFace6.setBackground(Color.BLACK);
		rdbtnFace6.setBounds(356, 442, 30, 25);
		contentPane.add(rdbtnFace6);
		
		JLabel lblFace1 = new JLabel(new ImageIcon("Face1.png"));
		lblFace1.setBounds(80, 348, 66, 60);
		contentPane.add(lblFace1);
		
		JLabel lblFace2 = new JLabel(new ImageIcon("Face2.png"));
		lblFace2.setBounds(231, 348, 66, 60);
		contentPane.add(lblFace2);
		
		JLabel lblFace3 = new JLabel(new ImageIcon("Face3.png"));
		lblFace3.setBounds(385, 348, 66, 60);
		contentPane.add(lblFace3);
		
		JLabel lblFace4 = new JLabel(new ImageIcon("Face4.png"));
		lblFace4.setBounds(80, 446, 66, 60);
		contentPane.add(lblFace4);
		
		JLabel lblFace5 = new JLabel(new ImageIcon("Face5.png"));
		lblFace5.setBounds(231, 442, 66, 60);
		contentPane.add(lblFace5);
		
		JLabel lblFace6 = new JLabel(new ImageIcon("Face6.png"));
		lblFace6.setBounds(385, 442, 66, 60);
		contentPane.add(lblFace6);
		
		JButton btnLogin = new JButton("Login");
		btnLogin.setFont(new Font("Kristen ITC", Font.BOLD, 20));
		btnLogin.setBackground(Color.GREEN);
		btnLogin.setBounds(168, 555, 145, 43);
		contentPane.add(btnLogin);
		
		JButton btnCancel = new JButton("Cancel");
		btnCancel.setFont(new Font("Kristen ITC", Font.BOLD, 20));
		btnCancel.setBackground(Color.GRAY);
		btnCancel.setBounds(356, 555, 133, 43);
		contentPane.add(btnCancel);
		
		JLabel lblImageUrl = new JLabel("Image URL:");
		lblImageUrl.setForeground(Color.WHITE);
		lblImageUrl.setBackground(Color.BLACK);
		lblImageUrl.setFont(new Font("Kristen ITC", Font.BOLD, 13));
		lblImageUrl.setBounds(205, 370, 91, 22);
		//contentPane.add(lblImageUrl); // starts invisible
		
		JTextField url = new JTextField();
		url.setForeground(Color.WHITE);
		url.setBackground(Color.GRAY);
		url.setBounds(12, 402, 477, 22);
		//contentPane.add(url);    // starts invisible
		
		
		ipField.addFocusListener(new FocusListener() {
			public void focusGained(FocusEvent e) {
				// nada
				System.out.println("gained");
			}
			public void focusLost(FocusEvent e) {
		    	  if (ipField.getText().matches("[0-9]{1,3}" + "." + "[0-9]{1,3}" + "." + "[0-9]{1,3}" + "." + "[0-9]{1,3}")){
		    		  // correct
		    	  }
		    	  else {
		    		  System.out.println("not correct input");
		    		  cutText(ipField);
		    	  }
		    	  System.out.println("lost");
			}
		});
                
                ipField.addActionListener(new ActionListener(){

                    public void actionPerformed(ActionEvent e){
                        
                        btnLogin.doClick();

                }});
		
                portField.addActionListener(new ActionListener(){

                    public void actionPerformed(ActionEvent e){
                        
                        btnLogin.doClick();

                }});
                
		userField.addFocusListener(new FocusListener() {
			public void focusGained(FocusEvent e) {
				// nada
				System.out.println("gained");
			}
			public void focusLost(FocusEvent e) {
		    	  if (userField.getText().matches("[0-9,a-z,A-Z,!,@,#,$,%,^,&,*,(,),_,-,+,=,~,`,:,;,\",\',<,>,?]{1,25}")){
		    		  // correct
		    	  }
		    	  else {
		    		  System.out.println("not correct input");
		    		  cutText(userField);
		    	  }
		    	  System.out.println("lost");
			}
		});
                
                userField.addActionListener(new ActionListener(){

                    public void actionPerformed(ActionEvent e){
                        
                        btnLogin.doClick();

                }});
                
		rdbtnImgURL.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent e) {
				System.out.println("listened");
				contentPane.remove(rdbtnFace1);
				contentPane.remove(rdbtnFace2);
				contentPane.remove(rdbtnFace3);
				contentPane.remove(rdbtnFace4);
				contentPane.remove(rdbtnFace5);
				contentPane.remove(rdbtnFace6);
				contentPane.remove(lblFace1);
				contentPane.remove(lblFace2);
				contentPane.remove(lblFace3);
				contentPane.remove(lblFace4);
				contentPane.remove(lblFace5);
				contentPane.remove(lblFace6);
				contentPane.add(url);
				contentPane.add(lblImageUrl);
                                isURL = true;
                                isFunFace = false;
				revalidate();
				repaint();
			}
		});
		rdbtnFunFace.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent e) {
				contentPane.add(rdbtnFace1);
				contentPane.add(rdbtnFace2);
				contentPane.add(rdbtnFace3);
				contentPane.add(rdbtnFace4);
				contentPane.add(rdbtnFace5);
				contentPane.add(rdbtnFace6);
				contentPane.add(lblFace1);
				contentPane.add(lblFace2);
				contentPane.add(lblFace3);
				contentPane.add(lblFace4);
				contentPane.add(lblFace5);
				contentPane.add(lblFace6);
				contentPane.remove(url);
				contentPane.remove(lblImageUrl);
                                isFunFace = true;
                                isURL = false;
				revalidate();
				repaint();
			}
		});
		url.addFocusListener(new FocusListener() {
			public void focusGained(FocusEvent e) {
				
			}
			public void focusLost(FocusEvent e) {
				if(isUrl(url.getText())) {
					//cool accept
				}
				else {
					cutText(url);
				}
			}
		});
		btnCancel.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent e) {
				dispose();
			}
		});
                btnLogin.addActionListener(new ActionListener() {
                        public void actionPerformed(ActionEvent e) {
                            if(userField.getText().equals("") || ipField.getText().equals("") || portField.getText().equals("")){
                                return;
                            }
                            username = userField.getText().trim();
                            IP = ipField.getText().trim();
                            imgURL = url.getText().trim();
                            port = Integer.parseInt(portField.getText().trim());
                            faceSelected = 1;
                            if(!isFunFace) {
                                faceSelected = 0;
                            }
                            else {
                                if(rdbtnFace1.isSelected()) {
                                    faceSelected = 1;
                                }
                                else if(rdbtnFace2.isSelected()) {
                                    faceSelected = 2;
                                }
                                else if(rdbtnFace3.isSelected()) {
                                    faceSelected = 3;
                                }
                                else if(rdbtnFace4.isSelected()) {
                                    faceSelected = 4;
                                }
                                else if(rdbtnFace5.isSelected()) {
                                    faceSelected = 5;
                                }
                                else if(rdbtnFace6.isSelected()) {
                                    faceSelected = 6;
                                }
                                else {
                                    faceSelected = 0;
                                }
                            }
                             
                            dispose();
                        }
                });
	}
	public void cutText(JTextField tf) {
		tf.setText("");;	
	}
	
	
	public boolean isUrl(String text){
		try{
			URL url = new URL(text);
			url.getPath();
			if(text.contains(".jpg") || text.contains(".png") || text.contains(".gif") || text.contains(".jpeg") || text.contains(".bmp")) {
				// good
			}
			else
				return false;
		}
		catch (MalformedURLException e) {
			return false;
		}
		return true;
        }
        
        public static String getUsername() {
            return username;
        }
        public static String getImgUrl() {
            return imgURL;
        }
        public static String getHost() {
            return IP;
        }
        public static int getFaceSelected() {
            return faceSelected;
        }
        public static int getPort() {
            return port;
        }
        
        public static boolean getUsingPicturePreset() {
            return isFunFace;
        }
        public static void setUsername(String s) {
            username = s;
        }
        public static void setImgUrl(String s) {
            imgURL = s;
        }
        public static void setHost(String s) {
            IP = s;
        }
        public static void setFaceSelected(int i) {
            faceSelected = i;
        }
        public static void setPort(int i) {
            port = i;
        }
}
