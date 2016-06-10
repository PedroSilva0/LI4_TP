using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice
{
    
    class facade
    {
        private LI4Entities data=new LI4Entities();

        public void registarRes(int id,string nome,string morada,string latitude,string longitude)
        {
            id = Convert.ToInt32(id);
            double latitude2 = Convert.ToDouble(latitude);
            double longitude2 = Convert.ToDouble(longitude);
            data.Estabelecimento.Add(new Estabelecimento()
            {
                id_est = id,
                latitude = latitude2,
                morada = morada,
                nome = nome,
                longitude = longitude2
            });
            data.SaveChanges();
        }

        public List<restauranteDTO> listarRestaurantes()
        {
            //var lista = (from t in data.tasks select new { t.id, t.name, type=t.taskType.name, username = t.user.name }).ToList(); 
            var lista = data.Estabelecimento.ToList();
            //this.dataGrid.ItemsSource = lista;

            //comboxBox
            var listRest = new List<restauranteDTO>();
            listRest.Add(new restauranteDTO() { id_est = 0, nome = "Todos Restaurante",morada="sitio",latitude=0,longitude=0 });
            var restList = (from u in data.Estabelecimento select new restauranteDTO { id_est = u.id_est, nome = u.nome,
                morada =u.morada,latitude=u.latitude,longitude=u.longitude }).ToList();
            listRest.AddRange(restList);
            return listRest;
            /*this.comboBox.ItemsSource = listUsers;
            this.comboBox.DisplayMemberPath = "name";
            this.comboBox.SelectedValuePath = "id";

            this.comboBox.SelectedIndex = 0;

            //Combobox (user) insert
            var listaByUser = (from t in data.users
                               select new { t.id, t.name }).ToList();
            var listaByTaskType = (from t in data.taskTypes
                                   select new { t.id, t.name }).ToList();
            this.comboBox1.ItemsSource = listaByTaskType;
            this.comboBox1.DisplayMemberPath = "name";
            this.comboBox1.SelectedValuePath = "id";
            this.comboBox1.SelectedIndex = 0;
            this.comboBox2.ItemsSource = listaByUser;
            this.comboBox2.DisplayMemberPath = "name";
            this.comboBox2.SelectedValuePath = "id";
            this.comboBox2.SelectedIndex = 0;*/
        }

        public List<restauranteDTO> listTeste()
        {
            //var lista = (from t in data.tasks select new { t.id, t.name, type=t.taskType.name, username = t.user.name }).ToList(); 
            var lista = data.Estabelecimento.ToList();
            //this.dataGrid.ItemsSource = lista;

            //comboxBox
            var listRest = new List<restauranteDTO>();
            listRest.Add(new restauranteDTO() { id_est = 0, nome = "Todos Restaurante", morada = "sitio", latitude = 0, longitude = 0 });
            var restList = (from u in data.Estabelecimento
                            select new restauranteDTO
                            {
                                id_est = u.id_est,
                                nome = u.nome,
                                morada = u.morada,
                                latitude = u.latitude,
                                longitude = u.longitude
                            }).ToList();
            listRest.AddRange(restList);
            return listRest;
            /*this.comboBox.ItemsSource = listUsers;
            this.comboBox.DisplayMemberPath = "name";
            this.comboBox.SelectedValuePath = "id";

            this.comboBox.SelectedIndex = 0;

            //Combobox (user) insert
            var listaByUser = (from t in data.users
                               select new { t.id, t.name }).ToList();
            var listaByTaskType = (from t in data.taskTypes
                                   select new { t.id, t.name }).ToList();
            this.comboBox1.ItemsSource = listaByTaskType;
            this.comboBox1.DisplayMemberPath = "name";
            this.comboBox1.SelectedValuePath = "id";
            this.comboBox1.SelectedIndex = 0;
            this.comboBox2.ItemsSource = listaByUser;
            this.comboBox2.DisplayMemberPath = "name";
            this.comboBox2.SelectedValuePath = "id";
            this.comboBox2.SelectedIndex = 0;*/
        }


    }
}
