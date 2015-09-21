using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            

            //int windowLength = 7;
            //upload to github
            using (var db = new wofraConnection())
            {
                
                var query = from p in db.paper
                            orderby p.Abstract
                            select p;
                
                foreach (var item in query)
                {
                    string[] wOfStemmedAbs = item.StemmedAbstract.Split(' ');

                    int cnt=0;

                    foreach (string word in wOfStemmedAbs)
                    {
                        cnt = cnt + 1;
                        using (var dbnode = new wofraConnection())
                        {

                            var newnode = new preNode { value = word, paperId = item.ID, priority = cnt, isTitle = false };

                                dbnode.preNode.Add(newnode);
                                dbnode.SaveChanges();
                            
                        }
                    }

                    string[] wOfStemmedTit = item.StemmedTitle.Split(' ');

                    int cntT = 0;

                    foreach (string word in wOfStemmedTit)
                    {
                        cntT = cntT + 1;
                        using (var dbnodeTitle = new wofraConnection())
                        {

                            var newnodeTitle = new preNode { value = word, paperId = item.ID, priority = cnt+1,isTitle=true };

                            dbnodeTitle.preNode.Add(newnodeTitle);
                            dbnodeTitle.SaveChanges();

                        }
                    }


                }



                
                
            }

            MessageBox.Show("youre awesome!");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            int wSize =int.Parse(txtWindowSize.Text);
            
            using (var db = new wofraConnection()) {

                var qryPaper = from p in db.paper
                               where p.AuthorId != 21 
                               where p.AuthorId != 5
                               where p.AuthorId != 8
                               where p.AuthorId != 12
                               where p.AuthorId != 28
                               where p.AuthorId != 13
                               select p;
                            
                foreach (var itemPaper in qryPaper)
                {
                    int pr = 0;
                    using (var dbLink = new wofraConnection()){
                        var qryPreNode = from pn in db.preNode
                                         where pn.paperId == itemPaper.ID
                                         where pn.isTitle==false
                                         orderby pn.priority
                                         select pn;
                       
                        int sizeArr = (from pn in db.preNode
                                         where pn.paperId == itemPaper.ID
                                         where pn.isTitle == false
                                         orderby pn.priority
                                         select pn).Count();

                        preNode[] pNodes = new preNode[sizeArr];

                        foreach (var itemPreNode in qryPreNode) {

                            pNodes[pr] = itemPreNode;
                            

                            if (pr == 0)
                            {
                                pr = pr + 1;
                            }
                            else {

                                if (pr < wSize)
                                {
                                    
                                    for (int i=pr; i > 0; i--)
                                    {

                                        var newPreLink = new preLink
                                        {
                                            distance = pNodes[pr].priority - pNodes[i - 1].priority,
                                            nodeID1 = pNodes[i - 1].nodeID,
                                            nodeID2 = pNodes[pr].nodeID,
                                            paperID = pNodes[i].paperId
                                        };

                                        dbLink.preLink.Add(newPreLink);
                                        dbLink.SaveChanges();
                                        //i = i + 1;
                                    }

                                    pr = pr + 1;

                                }
                                else {

                                    for (int i = pr; i > pr-wSize+1; i--)
                                    {

                                        var newPreLink = new preLink
                                        {
                                            distance = pNodes[pr].priority - pNodes[i - 1].priority,
                                            nodeID1 = pNodes[i - 1].nodeID,
                                            nodeID2 = pNodes[pr].nodeID,
                                            paperID = pNodes[i].paperId
                                        };

                                        dbLink.preLink.Add(newPreLink);
                                        dbLink.SaveChanges();
                                        
                                    }
                                    pr = pr + 1;
                                
                                }
                            }
                            
                            

                            
                            
                        }
                    }
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
