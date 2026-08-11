// <copyright file="LocaleTR.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: Localization/LocaleTR.cs
// Turkish tr-TR locale for Magic Hearse.

namespace MagicHearse
{
    using System.Collections.Generic; // IEnumerable, Dictionary, KeyValuePair
    using Colossal; // IDictionarySource, IDictionaryEntryError

    /// <summary>
    /// Turkish localization source for Magic Hearse [MH].</summary>
    public sealed class LocaleTR : IDictionarySource
    {
        private readonly MHSetting m_Setting;

        /// <summary>
        /// Constructs the Turkish locale generator.</summary>
        /// <param name="setting">Settings object used for locale IDs.</param>
        public LocaleTR(MHSetting setting)
        {
            m_Setting = setting;
        }

        /// <summary>
        /// Creates all Turkish localization entries for this mod.</summary>
        public IEnumerable<KeyValuePair<string, string>> ReadEntries(
            IList<IDictionaryEntryError> errors,
            Dictionary<string, int> indexCounts)
        {
            string title = Mod.kModName;

            if (!string.IsNullOrEmpty(Mod.ModVersion))
            {
                title = title + " (" + Mod.ModVersion + ")";
            }
            return new Dictionary<string, string>

            {
                // Options mod name
                { m_Setting.GetSettingsLocaleID(), title },

                // Tabs
                { m_Setting.GetOptionTabLocaleID(MHSetting.kActionsTab), "İşlemler" },
                { m_Setting.GetOptionTabLocaleID(MHSetting.kAboutTab), "Hakkında" },

                // Groups
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAutoCleanGrp), "Otomatik Temizleme" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kSelfManageGrp), "Kendin Yönet" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAdvancedGrp), "Gelişmiş" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kStatusGrp), "Durum" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAboutInfoGrp), "Mod bilgisi" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAboutLinksGrp), "Bağlantılar" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kDebugGrp), "Hata ayıklama" },

                // Auto Clean (magic)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.EnableMagicHearse)), "Sihirli Temizlemeyi etkinleştir" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.EnableMagicHearse)),
                    "Taşıma (cenaze aracı) gereken ölü bedenleri otomatik olarak kaldırır.\n" +
                    "Sihirli Temizleme ile kendin yönetme birbirini dışlar; birini seçin.\n" +
                    "Modu kaldırmadan devre dışı bırakmak için tüm onay kutularını kapatın.\n" +
                    "Teknik not: IsDead = true ve WaitingForHearse = true olmalıdır."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.MagicResetCemetery)), "Dolu mezarlığı sıfırla" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.MagicResetCemetery)),
                    "**Dolu bir mezarlığı boşaltır**, böylece DOLU simgesi nedeniyle engellenmez.\n" +
                    "Sihirli Temizleme çoğu bedeni gömülmeden önce kaldırır — bu seçenek **zaten dolu** olan mezarlıkları da boşaltır.\n" +
                    "<[ ] Varsayılan KAPALI>.\n" +
                    "Bu seçeneği yalnızca Sihirli Temizleme modunun zaten dolu mezarlıkları da boşaltmasını istiyorsanız etkinleştirin.\n" +
                    "Boşaltıldıktan sonra, Sihirli Temizleme açık kaldığı sürece genellikle bu seçeneği açık tutmaya gerek yoktur."
                },

                // Self Manage (FD)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.FuneralDirector)), "Cenaze müdürü" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.FuneralDirector)),
                    "Oyunun normal ölüm hizmetleri sistemlerini kendiniz yönetin ve optimize edin.\n" +
                    "**Ölçek değerleri:** hız, filo, depolama.\n" +
                    "İsteğe bağlı: **çalışan sayısını da artırın**."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ProcScalar)), "Krematoryum işlemesi" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ProcScalar)),
                    "**Krematoryum işleme hızı.**\n" +
                    "Daha yüksek değerler bedenleri daha hızlı kremasyon eder ve tesis depolamasını daha erken boşaltır.\n" +
                    "**100%** = oyunun varsayılan değeri."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.FleetScalar)), "Toplam cenaze aracı" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.FleetScalar)),
                    "Tesis başına **maksimum cenaze aracı**.\n" +
                    "**100%** = oyunun varsayılan değeri.\n" +
                    "**[Not]** Çok fazla cenaze aracı, ölüm oranına bağlı olarak trafiği etkileyebilir."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.HearseSpeedScalar)), "Cenaze aracı hızı" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.HearseSpeedScalar)),
                    "**Cenaze aracının izin verilen maksimum sürüş hızını artırır**.\n" +
                    "**100%** = oyunun varsayılan değeri.\n" +
                    "<Yol hız sınırları geçerli olmaya devam eder>.\n" +
                    "\n" +
                    "Yeni azami hızın aşırı kalkış/duruş davranışı oluşturmaması için hızlanma/frenlemeyi de yumuşak şekilde ölçekler.\n" +
                    "Not: Cenaze aracının azami hızı artırılsa bile gerçek sürüş hızı şunlardan etkilenir:\n" +
                    "araç için izin verilen azami hız, yol hız sınırı, oyunun yapay zekâ güvenli hızı (virajlar, yol hasarı) ve trafik."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.HearseWarningMinutes)), "Ölüm bildirimi gecikmesi (dk)" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.HearseWarningMinutes)),
                    "Bu, **cenaze aracı bekleniyor** sorun simgeleri görünmeden önce cenaze aracının bir binaya ulaşması için sahip olduğu toplam süredir.\n" +
                    "**3 dakika**, oyunun varsayılan yaklaşık 2,5 simülasyon dakikasına yakındır.\n" +
                    "Ölüm simgesi görünmeden önce cenaze araçlarına yolculuğu tamamlamaları için daha makul bir süre vermek üzere bu değeri artırabilirsiniz.\n" +
                    "Not:\n" +
                    "- <Önerilen: 10 dakika>. Çok yoğun şehirlerde daha yüksek bir değer deneyin.\n" +
                    "- Kaç vakanın geciktiğini görmek için alttaki Durum raporuna bakın.\n" +
                    "- Bu değer ilk kez artırıldığında zaten görünen simgeler gizlenmez; bir cenaze aracı çözüme ulaştırana veya bina yıkılana kadar kalırlar.\n" +
                    "- Mevcut sevklerin doğal şekilde bitmesini bekleyin veya yeni zamanlamalarla hızlı bir başlangıç için <Sihirli Temizleme [x]> kutusunu bir kez kullanın."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StorageScalar)), "Mezarlık depolaması" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StorageScalar)),
                    "Ana bina için **mezarlık depolama kapasitesi**.\n" +
                    "Daha fazla kapasite, dolu bir mezarlığın yeniden teslim almaya başlamasını sağlar.\n" +
                    "Alan eksikliği tesisi engellemiyorsa daha fazla cenaze aracı göndermez.\n" +
                    "**100%** = oyunun varsayılan değeri."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AutoResetCemetery)), "Mezarlığı otomatik sıfırla" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AutoResetCemetery)),
                    "**Dolu bir mezarlığı boşaltır**, böylece binanın üzerindeki DOLU simgesi nedeniyle engellenmez.\n" +
                    "Artık dolu mezarlıkları silip yeniden inşa etmeye gerek yok.\n" +
                    "Bunun yerine kademeli **Mezarlık devir hızını** kullanmak için bu seçeneği KAPATIN.\n" +
                    "<[ ✓ ] Varsayılan AÇIK>"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.CemeteryTurnoverScalar)), "Mezarlık devir hızı" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.CemeteryTurnoverScalar)),
                    "**Dolu mezar yerlerini kademeli olarak yeniden kullanılabilir hâle getirir.**\n" +
                    "Daha yüksek değerler mezar yerlerini temel oyuna göre daha hızlı kullanılabilir hâle getirir.\n" +
                    "Mezarlıklar 500% değerinde bile çok sık doluyorsa,\n" +
                    "bunun yerine **[Mezarlığı otomatik sıfırla]** seçeneğini etkinleştirin.\n" +
                    "**100%** = oyunun mezarları yeniden kullanma için varsayılan hızı."
                },

                // Workers compatibility toggle
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ControlWorkers)), "Çalışanları ayarla" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ControlWorkers)),
                    "Uyumluluk anahtarı:\n" +
                    "Çalışan sayısını artırmak için **Etkinleştir [✓]**.\n" +
                    "**[o_o]** Cenaze hizmeti çalışanlarını **ConfigXML** veya başka bir modun yönetmesini istiyorsanız KAPALI bırakın."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.WorkersScalar)), "Maksimum çalışan" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.WorkersScalar)),
                    "İzin verilen **maksimum çalışan sayısını artırır**.\n" +
                    "**100%** = oyunun varsayılan değeri."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ResetGameDefaults)), "Kaydırıcıları sıfırla" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ResetGameDefaults)), "Yüzde kaydırıcılarını **100%**, ölüm bildirimi gecikmesini **3 dakika** olarak ayarlar." },

                // STATUS fields (SHORT labels; left column is narrow!)

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary1)), "Cenaze aracı gerekli" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary1)),
                    "**Bekliyor** = hâlâ dışarıda olan ve alınmayı bekleyen tüm ölü vatandaşlar.\n" +
                    "**Gecikmiş** = seçilen bildirim gecikmesi sona ermiş bekleyen vatandaşlar.\n" +
                    " - Çok sayıda gecikmiş vaka varsa Ölüm bildirimi gecikmesi süresini artırmayı düşünün."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary2)), "Hacim" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary2)),
                    "Oyun istatistiklerinden **aylık toplamlar**.\n" +
                    "**Maks./ay** = mevcut verimlilikte krematoryum işlemesi + mezarlık devir hızı.\n" +
                    "Bu, tüm etkin cenaze hizmeti tesislerinin bir ayda işleyebileceği maksimum beden sayısıdır."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary3)), "Varlıklar" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary3)),
                    "**Etkin bina kapasiteleri:** toplam cenaze araçları, binalar, maksimum çalışanlar.\n" +
                    "\n" +
                    "**Notlar:**\n" +
                    "▪ Cenaze aracı: Etkin-park halinde değil / (Toplam* cenaze aracı)\n" +
                    "▪ *Toplam cenaze aracı:\n" +
                    "== bakımdaki cenaze araçlarını içerir (örn. düşük hizmet bütçesi), \n" +
                    "== devre dışı binalardaki cenaze araçlarını içermez.\n" +
                    "▪ Durum taraması yalnızca Seçenekler açıkken (veya bir kaydırıcı kullandığınızda) çalışır; şehirde her karede çalışmaz, bu yüzden performans etkisi neredeyse yoktur :)"
                },

                // Status text templates
                { "MH_STATUS_NOT_LOADED", "Durum yüklenmedi." },
                { "MH_STATUS_NO_CITY_LOADED", "Şehir yüklenmedi." },
                { "MH_STATUS_STATS_NOT_AVAIL", "Şehir yok... ¯\\_(ツ)_/¯ ...İstatistik yok" },

                { "MH_STATUS_LINE1_V2", "{0} bekliyor | {1} gecikmiş | {2} ölüm/ay" },
                { "MH_STATUS_LINE2_V2", "{0} maks./ay" },
                { "MH_STATUS_LINE3", "{0} / {1} cenaze aracı | {2} / {3} bina | {4} maks. çalışan" },
                { "MH_STATUS_UPDATED", "güncellendi {0}" },
                { "MH_STATUS_PROCESSING_SUGGESTED", "şimdi önerilen: ~{0}% krematoryum işlemesi" },
                { "MH_STATUS_PROCESSING_MORE", "şimdi önerilen: 500% krematoryum işlemesi + daha fazla etkin tesis" },
                { "MH_STATUS_PROCESSING_NONE", "önerilen: krematoryumları açın/ekleyin" },

                // Cemetery reset tally (session status; row + named list below Assets)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary4)), "Mezarlık" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary4)),
                    "Bu oturumdaki **kullanılan mezarları**, etkin mezarlık tesislerini ve dolu mezarlık sıfırlamalarını gösterir.\n" +
                    "Yeniden başlatmada veya şehir değiştirdiğinizde durum temizlenir."
                },

                { "MH_STATUS_LINE4_V2", "{0} / {1} mezar kullanılıyor | {2} tesis | {3}" },
                { "MH_STATUS_RESET_SINGULAR", "{0} sıfırlama" },
                { "MH_STATUS_RESET_PLURAL", "{0} sıfırlama" },
                { "MH_STATUS_CEMETERY_NONE", "bu oturumda yok" },
                { "MH_STATUS_CEMETERY_ROW", "{0} ×{1}" },
                { "MH_STATUS_CEMETERY_MORE", "+{0} daha" },

                // About
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AboutName)), "Mod" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AboutName)), "Bu modun görünen adı." },
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AboutVersion)), "Sürüm" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AboutVersion)), "Geçerli sürüm." },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.OpenParadoxMods)), "Paradox Mods" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.OpenParadoxMods)), "Yazarın Paradox Mods sayfasını açar." },

                // Debug report
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.LogReport)), "Günlük Raporu" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.LogReport)), "MagicHearse.log dosyasına ayrıntılı bir cenaze hizmeti raporu ve olası sorun alanlarını yazar." },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.OpenLog)), "Günlüğü Aç" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.OpenLog)),
                    "Varsa **Logs/MagicHearse.log** dosyasını açar.\n" +
                    "Dosya henüz yoksa bunun yerine Logs klasörünü açar."
                },
            };
        }

        public void Unload()
        { }
    }
}
