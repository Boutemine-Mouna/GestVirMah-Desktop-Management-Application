using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows;
using System.Data;
using System.Windows.Controls;

namespace GestVirMah.Classes

{
   
    class Demande
    {
        private SqlConnection conn;
       // public static SqlConnection conn = new SqlConnection(@"Data Source=DELL-PC;Initial Catalog=OeuvresSociales;Integrated Security=True;MultipleActiveResultSets=True");
       public Demande(SqlConnection con)
        {
            this.conn = con;
        }
        public int getMontant(String typeprime)
        { 
            try
            {
                conn.Open();
                SqlCommand cmdUser = new SqlCommand("select MontantPrime from TypePrime where DésignationPrime='" + typeprime + "'");
                cmdUser.Connection = conn;
                SqlDataReader ReadUser = cmdUser.ExecuteReader();
                ReadUser.Read();
                int i = int.Parse(ReadUser[0].ToString());
                return i;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to connect to data source" + ex.ToString());
                return 0;
            }
            finally
            {
                conn.Close();

            }

        }

        /**********************************************************************/
        /*****************************************************/

        public  List<String> typePrime()
        {
            
            List<String> l = new List<string>();
            String cmd = "select DésignationPrime from TypePrime";
            try
            {
                conn.Open();

                SqlCommand cmdUser = new SqlCommand(cmd, conn);
                SqlDataReader ReadUser = cmdUser.ExecuteReader();
                ReadUser.Read();
                while (ReadUser.Read())
                {
                    l.Add(ReadUser["DésignationPrime"].ToString());
                }

                return l;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to connect to data source" + ex.ToString());
                return null;
            }
            finally
            {
                conn.Close();

            }

        }

        /********************************************************************************/
        /*************************************************/

        public DataTable rechEtatDem(String etat)
        {
            String cmd = "select NomFonct,PrenFonct,EtatDem  from DemandePrime cross join Fonctionnaire where EtatDem ='" + etat + "'";
            try
            {
                conn.Open();
                SqlCommand cmdUser = new SqlCommand(cmd, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(cmdUser);
                DataTable t = new DataTable("Resultat");
                adapter.Fill(t);
                return t;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to connect to data source" + ex.ToString());
                return null;
            }
            finally
            {
                conn.Close();

            }

        }
        /***************************************************************************/
                 /******************************************************/
        public  DataTable fonctInfo(String nom, String prenom)
        {
            
            String cmd ="select SitFamFonct,TelFonct,Matricule,CompteFonct from Fonctionnaire where NomFonct= @nom and PrenFonct= @prenom";          
            try
            {
                conn.Open();
                SqlCommand cmdUser = new SqlCommand(cmd,conn);
                cmdUser.Parameters.AddWithValue("@nom", nom);
                cmdUser.Parameters.AddWithValue("@prenom", prenom);
                SqlDataAdapter adapter = new SqlDataAdapter(cmdUser);
                DataTable t = new DataTable("resultat");
                adapter.Fill(t);          
                return t;
            } 
            catch (Exception ex)
            {
                MessageBox.Show("Failed to connect to data source" + ex.ToString());
                return null;
            }
            finally
            {
                conn.Close();

            }
 
        }
        /**********************************************************************/
                 /*******************************************************/
        public  void creerDemande(String nom, String prenom, String montant, String codeUser, string typePrime, DatePicker dateEvent, DatePicker datDem,string etat)
        {
           
            String matricule = fonctInfo(nom, prenom).Rows[0]["Matricule"].ToString();          
            String codePrime = getCodePrime(typePrime);           
            String dateDem = (datDem.Text.Substring(6, 4) + "/" + datDem.Text.Substring(3, 3) + datDem.Text.Substring(0, 2)).ToString();           
            String compte = fonctInfo(nom, prenom).Rows[0]["CompteFonct"].ToString();
            String num = numDem(dateDem).ToString();         
            String even = (dateEvent.Text.Substring(6, 4) + "/" + dateEvent.Text.Substring(3, 3) + dateEvent.Text.Substring(0, 2)).ToString();
            String cmd = "insert into DemandePrime values (" + num + ",'" + dateDem + "'," + matricule + "," + codePrime + "," + montant + ",'" + compte + "','" + even + "','"+etat+"',null,null,GETDATE()," + codeUser + ")";
            try
            {
                conn.Open();
                SqlCommand cmdUser = new SqlCommand(cmd, conn);
                SqlDataReader reader = cmdUser.ExecuteReader();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to connect to data source" + ex.ToString());                
            }
            finally
            {
                conn.Close();
            }
        }
        /****************************************************/

        public  List<LigneNomPren> fonctionaire(String Nom)
        {
            List<LigneNomPren> l = new List<LigneNomPren>();
            String cmd = "select NomFonct,PrenFonct,CompteFonct from Fonctionnaire where NomFonct +' '+ PrenFonct like '" + Nom + "%' or PrenFonct +' '+ NomFonct like '" + Nom + "%'";
            try
            {
                conn.Open();
                SqlCommand cmdUser = new SqlCommand(cmd,conn);
                SqlDataReader reader = cmdUser.ExecuteReader();
                while (reader.Read())
                {
                    LigneNomPren ligne = new LigneNomPren();
                    ligne.nom = reader[0].ToString();
                    ligne.prenom = reader[1].ToString();
                    ligne.compte = reader[2].ToString();
                    l.Add(ligne);
                }

                return l;              
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to connect to data source" + ex.ToString());
                return null;
            }
            finally
            {
                conn.Close();

            }


        }
        /********************************************************/
        /*******************************************************/
        public String getCodePrime(String typeprime)
        {         
            try
            {
                conn.Open();
                SqlCommand cmdUser = new SqlCommand("select CodePrime from TypePrime where DésignationPrime='" + typeprime + "'");
                cmdUser.Connection = conn;
                SqlDataReader ReadUser = cmdUser.ExecuteReader();
                ReadUser.Read();
                return ReadUser["CodePrime"].ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to connect to data source" + ex.ToString());
                return null;
            }
            finally
            {
                conn.Close();

            }
        }
        /*************************************************************************/
          /************************************************************/
        public  String maxNumDem(String date)
        {
            String cmd = "select MAX(NumDem) as NumDem from DemandePrime where DateDem like '" + date + "%'";
            try
            {
                conn.Open();
                SqlCommand cmdUser = new SqlCommand(cmd, conn);
                SqlDataReader ReadUser = cmdUser.ExecuteReader();
                ReadUser.Read();

                if (ReadUser["NumDem"] != DBNull.Value)
                {
                    return ReadUser["NumDem"].ToString();
                }

                else return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to connect to data source" + ex.ToString());
                return null;
            }
            finally
            {
                conn.Close();

            }

        }
        /**************************************************************************/
                /***********************************************/
        public  int numDem(String d)
        {
            int date = int.Parse(d.Substring(2, 2));
            String date2 = d.Substring(0, 4);
            int maxNum = 0;
            if (maxNumDem(date2) == null)
            {
                maxNum = 0;
            }
            else
            {
                if (int.Parse(maxNumDem(date2).Substring(0, 2)) < date)
                {
                    maxNum = 0;
                }
                else
                {
                    maxNum = (int.Parse(maxNumDem(date2)) % 1000) + 1;
                }

            }
            int i = ((date * 1000) + maxNum);
            return i;
        }
        /*******************************************************************************/
        public DataTable batch_accepte()
        {

            try
            {
                conn.Open();
                SqlCommand cmd1 = new SqlCommand("SELECT * FROM Parametres ", conn);
                SqlDataReader read = cmd1.ExecuteReader();
                read.Read();
                string durcots = read[2].ToString();
                string jourd = read[3].ToString();
                int moideb = Convert.ToInt32(read[4].ToString());
                int year = Convert.ToInt32(DateTime.Now.Year);
                int month = Convert.ToInt32(DateTime.Now.Month);
                conn.Close();
                if (month >= moideb && month <= 12) { year -= 1; }
                else { year -= 1; }
                // La requete qui selectionne tout les fonctioonaires qui sont acceptes dans la prime generale
                //where (DateDepartDefi is  null and DateDepartDefi is null) or (DateDepartDefi is not null  and DATEDIFF(DAY," + year.ToString() + "/" + moideb.ToString() + "/" + jourd.ToString() + ",DateDepartDefi)>=" + durcot + " ) or (DateDepartTmp is not null and DateRetrTmp is not null and DATEDIFF(DAY," + year.ToString() + "/" + moideb.ToString() + "/" + jourd.ToString() + ",DateDepartTmp)>=" + durcot + " ) or ( DateDepartTmp is not null and DateRetrTmp is not null and DATEDIFF(DAY,DateDepartTmp,DateRetrTmp)>=0 ) or (DateDepartTmp is not null and DateRetrTmp is null and DATEDIFF(DAY," + year.ToString() + "/" + moideb.ToString() + "/" + jourd.ToString() + ",DateDepartTmp)>=" + durcot + "
                SqlCommand cmd = new SqlCommand("SELECT *FROM Fonctionnaire where  (DateDepartDefi is  null and DateDepartTmp is null) or (DateDepartDefi is not null  and DATEDIFF(DAY,'" + year.ToString() + "/" + moideb.ToString() + "/" + jourd.ToString() + "',DateDepartDefi)>=" + durcots + " ) or (DateDepartDefi is null and DateDepartTmp is not null and DateRetrTmp is not null and DATEDIFF(DAY,'" + year.ToString() + "/" + moideb.ToString() + "/" + jourd.ToString() + "',DateDepartTmp)>=" + durcots + " ) or ( DateDepartDefi is null and DateDepartTmp is not null and DateRetrTmp is not null and DATEDIFF(DAY,DateDepartTmp,DateRetrTmp)>=0 ) or (DateDepartDefi is null and DateDepartTmp is not null and DateRetrTmp is null and DATEDIFF(DAY,'" + year.ToString() + "/" + moideb.ToString() + "/" + jourd.ToString() + "',DateDepartTmp)>=" + durcots + ")", conn);
                DataTable dt = new DataTable("Fonctionnaire");
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
            finally
            {
                conn.Close();
           }

        }
        /*****************************************************************/
        public  Boolean isBatch(string prime)
        {
            string cmd = "select IsBatch from TypePrime where DésignationPrime ='" +prime+ "'";
            try
            {
                conn.Open();               
                SqlCommand cmdUser = new SqlCommand(cmd, conn);
                SqlDataReader ReadUser = cmdUser.ExecuteReader();
                ReadUser.Read();               
                if (ReadUser["IsBatch"].ToString() == "O") return true;                                 
                else return false;                
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            finally
            {
                conn.Close();
                
            }

        }
        /**********************************************************************/
        public  List<string> lastPv(DatePicker DatePV, string codeuser)
        {
            string cmd = "select * from PV where CodePV = (SELECT max(CodePV) as CodePv  FROM  PV  where IsVir='N')";

            try
            {
                conn.Open();
                SqlCommand cmd1 = new SqlCommand(cmd, conn);
                SqlDataReader reader = cmd1.ExecuteReader();
                reader.Read();
                List<string> l = new List<string>();

                if (reader.HasRows)
                {
                    l.Add(reader[0].ToString());
                    l.Add(reader[1].ToString());
                    return l;
                }
                else
                {
                    reader.Close();
                    conn.Close();
                    string date = DatePV.Text.Substring(6, 4) + "/" + DatePV.Text.Substring(3, 3) + DatePV.Text.Substring(0, 2);
                    int codePV = CodePV(date);
                    conn.Open();

                    String reqtPv = "insert into PV values( " + codePV.ToString() + ",'" + date + "',GETDATE()," + codeuser + ",'N')";
                    SqlCommand cmdAddPv = new SqlCommand(reqtPv, conn);
                    cmdAddPv.ExecuteNonQuery();
                    l.Add(codePV.ToString());
                    l.Add(date);
                    return l;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
            finally
            {
                conn.Close();
            }

        }
        /*public static List <string > addPvToBatch( DatePicker DatePV,int codeuser)
        {
            
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
                finally
                {
                    conn.Close();
                }
                return l; 
            }
        }
        */
        /*********************************************/

        public  void creerDemande(String nom, String prenom, String montant, String codeUser, string typePrime, DatePicker dateEvent, DatePicker datDem, string etat ,string codePv)
        {

            String matricule = fonctInfo(nom, prenom).Rows[0]["Matricule"].ToString();
            String codePrime = getCodePrime(typePrime);
            String dateDem = (datDem.Text.Substring(6, 4) + "/" + datDem.Text.Substring(3, 3) + datDem.Text.Substring(0, 2)).ToString();
            String compte = fonctInfo(nom, prenom).Rows[0]["CompteFonct"].ToString();
            String num = numDem(dateDem).ToString();
            String even = (dateEvent.Text.Substring(6, 4) + "/" + dateEvent.Text.Substring(3, 3) + dateEvent.Text.Substring(0, 2)).ToString();
            String cmd = "insert into DemandePrime values (" + num + ",'" + dateDem + "'," + matricule + "," + codePrime + "," + montant + ",'" + compte + "','" + even + "','" + etat + "',null,"+codePv+",GETDATE()," + codeUser + ")";
            try
            {
                conn.Open();
                SqlCommand cmdUser = new SqlCommand(cmd, conn);
                SqlDataReader reader = cmdUser.ExecuteReader();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to connect to data source" + ex.ToString());
            }
            finally
            {
                conn.Close();
            }
        }
        /***********************************************************************/
        public  String maxCodePV(String date)
        {
            String cmd = "select MAX(CodePV) as CodePV from PV where DatePV like '" + date + "%'";
           // SqlConnection con = new SqlConnection("Data Source=localhost;Initial Catalog=oeuvresSociales;Integrated Security=true; ");
            try
            {
                conn.Open();
                SqlCommand cmdCodePV = new SqlCommand(cmd, conn);
                SqlDataReader ReadUser = cmdCodePV.ExecuteReader();
                ReadUser.Read();

                if (ReadUser["CodePV"] != DBNull.Value)
                {
                    String s = ReadUser["CodePV"].ToString();
                    ReadUser.Close();
                    return s;
                }

                else return null;
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Failed to connect to data source" + ex.ToString());
                return null;
            }
            finally
            {
                conn.Close();
            }

        }

        /***********************************************/
        public  int CodePV(String d)
        {
            int date = int.Parse(d.Substring(2, 2));
            String date2 = d.Substring(0, 4);
            int maxCode = 0;
            if (maxCodePV(date2) == null)
            {
                maxCode = 0;
            }
            else
            {
                if (int.Parse(maxCodePV(date2).Substring(0, 2)) < date)
                {
                    maxCode = 0;
                }
                else
                {
                    maxCode = (int.Parse(maxCodePV(date2)) % 100) + 1;
                }

            }
            int i = ((date * 100) + maxCode);
            return i;
        }

        /**************************************************************************/
                /******************************************************/
        public  string converti(int chiffre)
        {
            int centaine, dizaine, unite, reste, y;
            bool dix = false;
            string lettre = "";
            //strcpy(lettre, "");

            reste = chiffre;

            for (int i = 1000000000; i >= 1; i /= 1000)
            {
                Console.WriteLine(i);
                y = reste / i;
                if (y != 0)
                {
                    centaine = y / 100;
                    dizaine = (y - centaine * 100) / 10;
                    unite = y - (centaine * 100) - (dizaine * 10);
                    switch (centaine)
                    {
                        case 0:
                            break;
                        case 1:
                            lettre += "cent ";
                            break;
                        case 2:
                            if ((dizaine == 0) && (unite == 0)) lettre += "deux cents ";
                            else lettre += "deux cent ";
                            break;
                        case 3:
                            if ((dizaine == 0) && (unite == 0)) lettre += "trois cents ";
                            else lettre += "trois cent ";
                            break;
                        case 4:
                            if ((dizaine == 0) && (unite == 0)) lettre += "quatre cents ";
                            else lettre += "quatre cent ";
                            break;
                        case 5:
                            if ((dizaine == 0) && (unite == 0)) lettre += "cinq cents ";
                            else lettre += "cinq cent ";
                            break;
                        case 6:
                            if ((dizaine == 0) && (unite == 0)) lettre += "six cents ";
                            else lettre += "six cent ";
                            break;
                        case 7:
                            if ((dizaine == 0) && (unite == 0)) lettre += "sept cents ";
                            else lettre += "sept cent ";
                            break;
                        case 8:
                            if ((dizaine == 0) && (unite == 0)) lettre += "huit cents ";
                            else lettre += "huit cent ";
                            break;
                        case 9:
                            if ((dizaine == 0) && (unite == 0)) lettre += "neuf cents ";
                            else lettre += "neuf cent ";
                            break;
                    }// endSwitch(centaine)

                    switch (dizaine)
                    {
                        case 0:
                            break;
                        case 1:
                            dix = true;
                            break;
                        case 2:
                            lettre += "vingt ";
                            break;
                        case 3:
                            lettre += "trente ";
                            break;
                        case 4:
                            lettre += "quarante ";
                            break;
                        case 5:
                            lettre += "cinquante ";
                            break;
                        case 6:
                            lettre += "soixante ";
                            break;
                        case 7:
                            dix = true;
                            lettre += "soixante ";
                            break;
                        case 8:
                            lettre += "quatre-vingt ";
                            break;
                        case 9:
                            dix = true;
                            lettre += "quatre-vingt ";
                            break;
                    } // endSwitch(dizaine)

                    switch (unite)
                    {
                        case 0:
                            if (dix) lettre += "dix ";
                            break;
                        case 1:
                            if (dix) lettre += "onze ";
                            else lettre += "un ";
                            break;
                        case 2:
                            if (dix) lettre += "douze ";
                            else lettre += "deux ";
                            break;
                        case 3:
                            if (dix) lettre += "treize ";
                            else lettre += "trois ";
                            break;
                        case 4:
                            if (dix) lettre += "quatorze ";
                            else lettre += "quatre ";
                            break;
                        case 5:
                            if (dix) lettre += "quinze ";
                            else lettre += "cinq ";
                            break;
                        case 6:
                            if (dix) lettre += "seize ";
                            else lettre += "six ";
                            break;
                        case 7:
                            if (dix) lettre += "dix-sept ";
                            else lettre += "sept ";
                            break;
                        case 8:
                            if (dix) lettre += "dix-huit ";
                            else lettre += "huit ";
                            break;
                        case 9:
                            if (dix) lettre += "dix-neuf ";
                            else lettre += "neuf ";
                            break;
                    } // endSwitch(unite)

                    switch (i)
                    {
                        case 1000000000:
                            if (y > 1) lettre += "milliards ";
                            else lettre += "milliard ";
                            break;
                        case 1000000:
                            if (y > 1) lettre += "millions ";
                            else lettre += "million ";
                            break;
                        case 1000:
                            if (y > 1) lettre += "milles ";
                            else lettre = "mille ";
                            break;
                    }
                } // end if(y!=0)
                reste -= y * i;
                dix = false;
            } // end for
            if (lettre.Length == 0) lettre += "zero";

            return lettre;
        }


    }
}
