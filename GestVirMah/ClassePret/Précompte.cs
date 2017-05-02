using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data;
using System.Windows;

namespace GestVirMah.ClassePret
{
    public class Précompte
    {
        private float montant;
        private int NumDemPret;
        SqlConnection  con;
        private DataRow ligne;
        public Précompte(int NumDemPret, SqlConnection con)
        {
            this.NumDemPret = NumDemPret;
            this.con = con;      
        }

        /*-------------------------------------------------------------------les méthode---------------------------------------------------------------*/
        public Boolean istype2()
        {
            this.con.Open(); Boolean isT2 = false;
            SqlCommand cmd1 = new SqlCommand("Select CodePret FROM DemandePret WHERE NumDemPret=@numd ", this.con);
            cmd1.Parameters.Add("@numd", this.NumDemPret);
            DataTable tab = new DataTable();
            SqlDataAdapter ad1 = new SqlDataAdapter(cmd1);
            ad1.Fill(tab);
            this.con.Close();
            int code = int.Parse(tab.Rows[0]["CodePret"].ToString());
            this.con.Open();
            SqlCommand cmd2 = new SqlCommand("Select TypePret from TypePret where CodePret = @code", this.con);
            cmd2.Parameters.Add("@code", code);
            DataTable t = new DataTable();
            SqlDataAdapter ad2 = new SqlDataAdapter(cmd2);
            ad2.Fill(t);
            this.con.Close();
            int type = int.Parse(t.Rows[0]["TypePret"].ToString());
            if (type==2)
            {
                this.con.Open();
                SqlCommand cmd3 = new SqlCommand("Select Apport FROM DemandePret WHERE NumDemPret=@numd ", this.con);
                cmd3.Parameters.Add("@numd", this.NumDemPret);
                DataTable ta = new DataTable();
                SqlDataAdapter ad3 = new SqlDataAdapter(cmd3);
                ad3.Fill(ta);
                this.con.Close();
                float app = float.Parse(ta.Rows[0]["Apport"].ToString());
                if (app != 0) isT2 = true;
            }
           
            return isT2;
        }
             
        public Boolean verifDate()
        {  this.con.Open();
           SqlCommand cmd = new SqlCommand("Select TOP(1) * FROM RemboursementAnticipe WHERE (DateAnt<=@date2)AND(PremiereEcheanceAnt<=@date2)AND(NumDemPret=@numd) ORDER BY DateAnt DESC", this.con);
           cmd.Parameters.Add("@numd", this.NumDemPret);
           cmd.Parameters.Add("@date2", SqlDbType.Date).Value = DateTime.Now.Date.ToString();
           DataTable tabRem = new DataTable();
           SqlDataAdapter ad1 = new SqlDataAdapter(cmd);
           ad1.Fill(tabRem);
           this.con.Close();
           if (tabRem.Rows.Count == 0)
           {
               this.con.Open();
               SqlCommand cmdEcheance = new SqlCommand("Select * FROM DemandePret WHERE (NumDemPret=@nu) AND (PremiereEcheance<= @date)", this.con);
               cmdEcheance.Parameters.Add("@nu", this.NumDemPret);
               cmdEcheance.Parameters.Add("@date", SqlDbType.Date).Value = DateTime.Now.Date.ToString();
               DataTable ech = new DataTable();
               SqlDataAdapter ad2 = new SqlDataAdapter(cmdEcheance);
               ad2.Fill(ech);
               this.con.Close();
               if (ech.Rows.Count == 0) return false;
               else return true;
              
           }
           else
           {
               return true;
           }
    
       }
        public Boolean ligneAconsi()  //Si c false c la ligne de la dem qui est mise dans ligne, sinon c la ligne du rem anti concerné  
        {
            /*this.con.Open();
            SqlCommand cmd = new SqlCommand("Select TOP(1) * FROM RemboursementAnticipe WHERE (DateAnt<=@date2)AND(PremiereEcheanceAnt<=@date2)AND(NumDemPret=@numd) ORDER BY DateAnt DESC",this.con);
            cmd.Parameters.Add("@numd", this.NumDemPret);
            cmd.Parameters.Add("@date2", SqlDbType.Date).Value = DateTime.Now.Date.ToString();
            DataTable tabRem = new DataTable();
            SqlDataAdapter ad1 = new SqlDataAdapter(cmd);
            ad1.Fill(tabRem);
            this.con.Close();
            if (tabRem.Rows.Count == 0)
            {
                this.con.Open();
                SqlCommand cmdEcheance = new SqlCommand("Select * FROM DemandePret WHERE NumDemPret=@nu", this.con);
                cmdEcheance.Parameters.Add("@nu", this.NumDemPret);
                //cmdEcheance.Parameters.Add("@date", SqlDbType.Date).Value = DateTime.Now.Date.ToString();
                DataTable ech = new DataTable();
                SqlDataAdapter ad2 = new SqlDataAdapter(cmdEcheance);
                ad2.Fill(ech);
                this.con.Close();
                this.ligne = ech.NewRow();
                this.ligne.ItemArray = (object[])ech.Rows[0].ItemArray.Clone();
                return false;
            }
            else
            {
                    this.ligne = tabRem.NewRow();
                    this.ligne.ItemArray = (object[])tabRem.Rows[0].ItemArray.Clone();
                    return true;
                }*/
            con.Open();
            SqlCommand cmdDernierPre = new SqlCommand("Select TOP(1) DatePrecompte FROM Précompte WHERE NumDemPret=@num ORDER BY DatePrecompte DESC", con);
            cmdDernierPre.Parameters.Add("@num", this.NumDemPret);
            DataTable dt = new DataTable();
            SqlDataAdapter ad = new SqlDataAdapter(cmdDernierPre);
            ad.Fill(dt);
            con.Close();
            if (dt.Rows.Count == 0)
            {
                con.Open();
                SqlCommand cmdEcheance = new SqlCommand("Select * FROM DemandePret WHERE NumDemPret=@nu", con);
                cmdEcheance.Parameters.Add("@nu", this.NumDemPret);
                DataTable ech = new DataTable();
                SqlDataAdapter ad2 = new SqlDataAdapter(cmdEcheance);
                ad2.Fill(ech);
                con.Close();
                this.ligne = ech.NewRow();
                this.ligne.ItemArray = (object[])ech.Rows[0].ItemArray.Clone();
                return false;
            }
            else
            {
                con.Open();
                DateTime date = Convert.ToDateTime((dt.Rows[0][0]).ToString());
                SqlCommand cmd = new SqlCommand("Select TOP(1) * FROM RemboursementAnticipe WHERE (DateAnt<@date1)AND(PremiereEcheanceAnt<=@date2)AND(NumDemPret=@numd) ORDER BY DateAnt DESC", con);
                cmd.Parameters.Add("@numd", this.NumDemPret);
                cmd.Parameters.Add("@date1", SqlDbType.Date).Value = date.ToString();
                cmd.Parameters.Add("@date2", SqlDbType.Date).Value = DateTime.Now.Date.ToString();
                DataTable tabRem = new DataTable();
                SqlDataAdapter ad1 = new SqlDataAdapter(cmd);
                ad.Fill(tabRem);
                con.Close();
                if (tabRem.Rows.Count == 0)
                {
                    con.Open();
                    SqlCommand cmdEcheance = new SqlCommand("Select * FROM DemandePret WHERE NumDemPret=@nu", con);
                    cmdEcheance.Parameters.Add("@nu", this.NumDemPret);
                    DataTable ech = new DataTable();
                    SqlDataAdapter ad2 = new SqlDataAdapter(cmdEcheance);
                    ad2.Fill(ech);
                    con.Close();
                    this.ligne = ech.NewRow();
                    this.ligne.ItemArray = (object[])ech.Rows[0].ItemArray.Clone();
                    return false;
                }
                else
                {
                    this.ligne = tabRem.NewRow();
                    this.ligne.ItemArray = (object[])tabRem.Rows[0].ItemArray.Clone();
                    return true;
                }
            }
             
            
        }
        public Boolean consPrec()
        {
            int per = 0; int p = 0; int month; int dif;
            Boolean lig = ligneAconsi();
            this.con.Open();
            SqlCommand cmd = new SqlCommand("SELECT DatePrecompte FROM Précompte WHERE NumDemPret=@num ORDER BY DatePrecompte DESC", con);
            cmd.Parameters.Add("@num", this.NumDemPret);
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            ad.Fill(dt);
            this.con.Close();
            if (dt.Rows.Count != 0)
            {
                if ((istype2() == true) && (dt.Rows.Count == 1)) return (true);
                else
                {
                    DateTime date = Convert.ToDateTime((dt.Rows[0][0]).ToString());
                    if (lig) per = int.Parse(this.ligne["PeriodiciteAnt"].ToString());
                    else per = int.Parse(this.ligne["Periodicite"].ToString());
                    p = date.Month + per;
                    if (p > 12)
                    {
                        if (DateTime.Now.Month < date.Month)
                        {
                            month = DateTime.Now.Month + 12;
                            dif = month - date.Month;
                        }
                        else dif = DateTime.Now.Month - date.Month;
                    }
                    else
                        dif = DateTime.Now.Month - date.Month;
                    if (dif >= per) return (true);
                    else return (false);
                }
            }
            else
           
                return (true);
        }

       public float montRet()
        {
            Boolean b = ligneAconsi();
            float monRestant;
            this.con.Open();
            SqlCommand mona = new SqlCommand("Select MontantAcc FROM DemandePret WHERE NumDemPret=@num",this.con);
            mona.Parameters.Add("@num", this.NumDemPret);
            DataTable d = new DataTable();
            SqlDataAdapter ad2 = new SqlDataAdapter(mona);
            ad2.Fill(d);
            this.con.Close();
            monRestant = float.Parse(d.Rows[0][0].ToString()) - nbPrecompte();

            if (b)
            {
                if (monRestant <= Convert.ToDouble(this.ligne["RetenuPerAnt"])) this.montant = monRestant;
                else this.montant = float.Parse(this.ligne["RetenuPerAnt"].ToString());
                return (this.montant);
            }
            else
            {
                if (monRestant <= Convert.ToDouble(this.ligne["RetenuPeriodique"])) this.montant = monRestant;
                else this.montant = float.Parse(this.ligne["RetenuPeriodique"].ToString());
                return (this.montant);
            }
       }
      


     public void upDatePrecompte(float montant,string motif)
     {  try
          {   
             DateTime thisDay = DateTime.Today.Date ;
             String cmd = "INSERT INTO Précompte (DatePrecompte,MontantPrecompte,MotifPrecpte,NumDemPret) VALUES (@datedem,'" + montant + "','"+motif+"','" + this.NumDemPret + "')";
             this.con.Open();
             SqlCommand cmdUser = new SqlCommand(cmd, this.con);
             cmdUser.Parameters.Add("@datedem", SqlDbType.Date).Value = thisDay.ToString();
             SqlDataReader reader = cmdUser.ExecuteReader();
             this.con.Close();
          }

        catch (Exception ex)
         {
              MessageBox.Show("Failed to connect to data source" + ex.ToString());
         }
        finally
         {
              this.con.Close();
         }
        
     }
      public float nbPrecompte()
     {  
          SqlCommand cmd = new SqlCommand("Select MontantPrecompte from Précompte where NumDemPret= @NumDemPret ",this.con); 
          cmd.Parameters.AddWithValue("@NumDemPret", this.NumDemPret);
          this.con.Open();
          float mont=0, Montant = 0;
          SqlDataAdapter adapter = new SqlDataAdapter(cmd);
          DataTable t = new DataTable();
          adapter.Fill(t);
          this.con.Close();
          if (t.Rows.Count!=0)
          {
              foreach (DataRow row in t.Rows)
            { 
              mont = float.Parse(row["MontantPrecompte"].ToString());
              Montant=Montant+mont;
            }
             return(Montant);
          }
          else  return(0);
    }
     public bool setEtat() 
     {
         float MontantRem = nbPrecompte();
         bool set = false;
         SqlCommand cmd = new SqlCommand("Select MontantAcc from DemandePret where NumDemPret= @NumDemPret ", this.con);
         cmd.Parameters.AddWithValue("@NumDemPret", this.NumDemPret);
         this.con.Open();
         SqlDataReader ReadUser = cmd.ExecuteReader();
         ReadUser.Read();
         float Montantacc = float.Parse(ReadUser["MontantAcc"].ToString());
         this.con.Close();
         if (MontantRem >= Montantacc)
         {
             String etat = "B";
             String cmd2 = "UPDATE DemandePret SET Etat= '"+etat+"' WHERE NumDemPret='" + this.NumDemPret + "'";
             this.con.Open();
             SqlCommand cmdUser = new SqlCommand(cmd2, this.con);
             SqlDataReader reader2 = cmdUser.ExecuteReader();
             this.con.Close();
             set = true;
         }
         return (set);
     }
   
    }
}
