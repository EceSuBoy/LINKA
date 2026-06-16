using System.Text;

namespace Linka.IdentityServer
{
    /*
     * SeedData sınıfının ikinci parçası (partial class).
     *
     * Buradaki amaç, mevcut Identity veritabanını SIFIRLAMADAN
     * 100 adet gerçekçi test kullanıcısını eklemektir.
     *
     * Çalışma mantığı:
     *  - Var olan hesaplar (linkaadmin, linkamanager, kerem, mehmet,
     *    ayse ve daha önce kayıt olmuş tüm gerçek kullanıcılar) ASLA
     *    silinmez. EnsureSeedData içindeki CreateOrUpdateUser metodu
     *    kullanıcıyı username üzerinden arar; yoksa oluşturur, varsa
     *    sadece günceller. Yani bu seed birden çok kez çalıştırılsa
     *    bile kullanıcılar tekrar tekrar eklenmez.
     *
     * Kurallar (istenildiği gibi):
     *  - Tüm test kullanıcılarının şifresi: 1111aA*
     *  - İsim ve soyisimler birbirinden farklı, gerçekçi ve
     *    Türkçe + İngilizce karışık.
     *  - E-posta: (isim)@linka.com          -> ör. ahmet@linka.com
     *  - Username: (isim)01                 -> ör. ahmet01
     *  - Rol: Customer (normal kullanıcı)
     *
     * Türkçe karakterler (ç, ğ, ı, İ, ö, ş, ü) e-posta ve username
     * içinde sorun çıkarmaması için ToSlug ile ASCII'ye çevrilir.
     * Görünen ad (Name) ise orijinal Türkçe haliyle saklanır.
     */
    public partial class SeedData
    {
        private const string TestUserPassword = "1111aA*";

        private static SeedUser[] GetTestUsers()
        {
            // 100 farklı ad (Türkçe + İngilizce karışık).
            // Adlar benzersiz seçildiği için üretilen e-posta ve
            // username değerleri de benzersiz olur.
            var firstNames = new[]
            {
                "Ahmet", "Mustafa", "Emir", "Yusuf", "Berk",
                "Can", "Deniz", "Efe", "Kaan", "Burak",
                "Cem", "Onur", "Tolga", "Serkan", "Barış",
                "Ozan", "Arda", "Mert", "Eren", "Selim",
                "Volkan", "Hakan", "Sinan", "Umut", "Görkem",
                "Çağatay", "Gökhan", "Furkan", "Doruk", "Ege",
                "Elif", "Zeynep", "Defne", "Buse", "Selin",
                "Ece", "Melis", "Derya", "İrem", "Sıla",
                "Pınar", "Esra", "Ceren", "Aslı", "Yağmur",
                "Damla", "Eylül", "Beren", "Öykü", "Çağla",
                "Şule", "Gizem", "Merve", "Tuğçe", "Cansu",
                "Ezgi", "James", "Oliver", "William", "Henry",
                "Jack", "Thomas", "George", "Charlie", "Edward",
                "Daniel", "Michael", "David", "Ryan", "Lucas",
                "Nathan", "Samuel", "Adam", "Ethan", "Mason",
                "Logan", "Connor", "Dylan", "Owen", "Liam",
                "Noah", "Caleb", "Emma", "Olivia", "Sophia",
                "Isabella", "Mia", "Charlotte", "Amelia", "Harper",
                "Evelyn", "Abigail", "Emily", "Grace", "Chloe",
                "Lily", "Zoe", "Hannah", "Ella", "Scarlett"
            };

            // 100 farklı soyad (Türkçe + İngilizce karışık).
            var lastNames = new[]
            {
                "Yılmaz", "Demir", "Çelik", "Kaya", "Şahin",
                "Yıldız", "Aydın", "Öztürk", "Arslan", "Doğan",
                "Kılıç", "Aslan", "Çetin", "Korkmaz", "Şen",
                "Güneş", "Polat", "Koç", "Kurt", "Özdemir",
                "Acar", "Bulut", "Taş", "Çakır", "Yavuz",
                "Toprak", "Güler", "Aksoy", "Tekin", "Ünal",
                "Çınar", "Karaca", "Sezer", "Eroğlu", "Kara",
                "Avcı", "Duman", "Erdem", "Bozkurt", "Şimşek",
                "Tunç", "Yalçın", "Kaplan", "Özkan", "Akın",
                "Soylu", "Başaran", "Coşkun", "Demirtaş", "Aktaş",
                "Smith", "Johnson", "Williams", "Brown", "Jones",
                "Miller", "Davis", "Wilson", "Moore", "Taylor",
                "Anderson", "Jackson", "White", "Harris", "Martin",
                "Thompson", "Clark", "Lewis", "Walker", "Hall",
                "Allen", "Young", "King", "Wright", "Scott",
                "Green", "Baker", "Adams", "Nelson", "Carter",
                "Mitchell", "Roberts", "Turner", "Parker", "Collins",
                "Edwards", "Morgan", "Murphy", "Cooper", "Bailey",
                "Reed", "Cook", "Bell", "Ward", "Brooks",
                "Gray", "Hughes", "Price", "Foster", "Bennett"
            };

            var users = new SeedUser[firstNames.Length];

            for (var i = 0; i < firstNames.Length; i++)
            {
                var slug = ToSlug(firstNames[i]);

                users[i] = new SeedUser
                {
                    Username = slug + "01",
                    Email = slug + "@linka.com",
                    Name = firstNames[i],
                    Surname = lastNames[i],
                    Password = TestUserPassword,
                    Role = "Customer"
                };
            }

            return users;
        }

        /*
         * Türkçe karakterleri ASCII'ye çevirip küçük harfe indirger.
         * Örn: "Çağla" -> "cagla", "İrem" -> "irem", "Şule" -> "sule".
         * İngilizce adlar olduğu gibi küçük harfe çevrilir.
         */
        private static string ToSlug(string value)
        {
            var builder = new StringBuilder(value.Length);

            foreach (var ch in value)
            {
                switch (ch)
                {
                    case 'ç':
                    case 'Ç':
                        builder.Append('c');
                        break;

                    case 'ğ':
                    case 'Ğ':
                        builder.Append('g');
                        break;

                    case 'ı':
                    case 'I':
                    case 'İ':
                    case 'i':
                        builder.Append('i');
                        break;

                    case 'ö':
                    case 'Ö':
                        builder.Append('o');
                        break;

                    case 'ş':
                    case 'Ş':
                        builder.Append('s');
                        break;

                    case 'ü':
                    case 'Ü':
                        builder.Append('u');
                        break;

                    case 'â':
                        builder.Append('a');
                        break;

                    case 'î':
                        builder.Append('i');
                        break;

                    case 'û':
                        builder.Append('u');
                        break;

                    default:
                        builder.Append(char.ToLowerInvariant(ch));
                        break;
                }
            }

            return builder.ToString();
        }
    }
}
