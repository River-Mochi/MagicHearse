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
                { m_Setting.GetOptionTabLocaleID(MHSetting.kActionsTab), "Eylemler" },
                { m_Setting.GetOptionTabLocaleID(MHSetting.kAboutTab), "Hakkında" },

                // Groups
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAutoCleanGrp),   "Otomatik Temizleme" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kSelfManageGrp),  "Kendi Yönetimin" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAdvancedGrp),    "Gelişmiş" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kStatusGrp),      "Durum" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAboutInfoGrp),   "Mod bilgisi" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAboutLinksGrp),  "Bağlantılar" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kDebugGrp),       "Hata ayıklama" },

                // Auto Clean (magic)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.EnableMagicHearse)), "Sihirli Temizlemeyi etkinleştir" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.EnableMagicHearse)),
                    "Taşınması gereken ölülerin cesetlerini otomatik olarak kaldırır (cenaze aracı).\n" +
                    "Sihirli temizleme ile kendi yönetimin birbirini dışlar; birini seçin.\n"+
                    "Modu kaldırmadan devre dışı bırakmak için tüm onay kutularını kapatın.\n"+
                    "Teknik not: IsDead = true ve WaitingForHearse = true olmalıdır."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.MagicResetCemetery)), "Dolu mezarlığı sıfırla" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.MagicResetCemetery)),
                    "**Dolu bir mezarlığı boşaltır**; böylece DOLU simgesiyle engellenmez.\n" +
                    "Sihirli Temizleme, cenazeden önce cesetlerin çoğunu kaldırır — bu seçenek **zaten dolu** olan mezarlıkları yine de temizler.\n" +
                    "<[ ] Varsayılan KAPALI>.\n" +
                    "Sihirli temizleme modu zaten dolu mezarlıkları da boşaltsın istiyorsanız bunu etkinleştirin.\n" +
                    "Mezarlık boşaltıldıktan sonra, sihirli temizleme açık kaldığı sürece genellikle bunu açık tutmanız gerekmez."
                },

                // Self Manage (FD)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.FuneralDirector)), "Cenaze Müdürü" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.FuneralDirector)),
                    "Oyunun normal ölüm hizmetlerini kendiniz yönetin ve iyileştirin.\n" +
                    "**Ölçeklenen değerler:** hız, filo, depolama.\n" +
                    "İsteğe bağlı: **çalışan sayısını da artırın.**"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ProcScalar)), "Krematoryum işleme" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ProcScalar)),
                    "**Krematoryum işleme hızı.**\n" +
                    "Daha yüksek değerler cesetleri daha hızlı yakar ve tesisin depolama alanını daha erken boşaltır.\n" +
                    "**100%** = oyunun varsayılan değeri."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.FleetScalar)), "Toplam cenaze aracı" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.FleetScalar)),
                    "Tesis başına **azami cenaze aracı**.\n" +
                    "**100%** = oyunun varsayılan değeri.\n" +
                    "**[Not]** Çok fazla cenaze aracı, ölüm oranına bağlı olarak trafiği etkileyebilir."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.HearseSpeedScalar)), "Cenaze aracı hızı" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.HearseSpeedScalar)),
                    "**Cenaze aracının izin verilen azami sürüş hızını artırır**.\n" +
                    "**100%** = oyunun varsayılan değeri.\n" +
                    "<Yol hız sınırları yine geçerlidir>.\n\n" +
                    "Yeni azami hız aşırı hızlanma veya durma davranışı yaratmasın diye hızlanma ve frenlemeyi de daha yumuşak biçimde ölçekler.\n" +
                    "Not: Cenaze aracının azami hızı artırılsa bile gerçek sürüş hızı şunlardan etkilenir:\n" +
                    "aracın izin verilen azami hızı, yol hız sınırı, oyunun kendi YZ güvenli hızı (virajlar, yol hasarı) ve trafik."

                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.HearseWarningMinutes)), "Cenaze aracı uyarı gecikmesi (dk)" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.HearseWarningMinutes)),
                    "**Cenaze aracı bekleniyor sorun simgesi görünmeden önceki simülasyon dakikası.**\n" +
                    "**3 dakika**, yaklaşık 2,5 dakikalık varsayılan değere yakındır.\n" +
                    "Yalnızca cenaze aracı uyarıları değişir; ambulans uyarıları oyun ayarında kalır.\n" +
                    "Değer artırıldığında zaten görünen simgeler gizlenmez."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StorageScalar)), "Mezarlık kapasitesi" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StorageScalar)),
                    "Ana binanın **mezarlık depolama kapasitesi**.\n" +
                    "Daha yüksek kapasite, dolu bir mezarlığın yeniden cenaze kabul etmesini sağlar.\n" +
                    "Alan yetersizliği tesisi engellemediği sürece daha fazla cenaze aracı göndermez.\n" +
                    "**100%** = oyunun varsayılan değeri."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AutoResetCemetery)), "Mezarlığı otomatik sıfırla" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AutoResetCemetery)),
                    "**Dolu bir mezarlığı boşaltır**; böylece binanın üzerindeki DOLU simgesi nedeniyle engellenmez.\n" +
                    "Artık dolu mezarlıkları silip yeniden inşa etmeniz gerekmez.\n" +
                    "Yerine kademeli **Mezar yeri yenilenme hızını** kullanmak için bunu KAPATIN.\n" +
                    "<[ ✓ ] Varsayılan AÇIK>"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.CemeteryTurnoverScalar)), "Mezar yeri yenilenme hızı" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.CemeteryTurnoverScalar)),
                    "**Kullanımdaki mezar yerlerini kademeli olarak yeniden boşaltır.**\n" +
                    "Daha yüksek değerler mezar yerlerini standart oyundan daha hızlı yeniden kullanılabilir hâle getirir.\n" +
                    "Mezarlıklar 500%'de hâlâ çok sık doluyorsa bunun yerine **[Mezarlığı otomatik sıfırla]** seçeneğini etkinleştirin.\n" +
                    "**100%** = oyunun varsayılan değeri."
                },

                // Workers compatibility toggle
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ControlWorkers)), "Çalışanları ayarla" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ControlWorkers)),
                    "Uyumluluk anahtarı:\n" +
                    "Çalışan sayısını artırmak için **Etkinleştirin [✓]**.\n" +
                    "Ölüm hizmeti çalışanlarını **ConfigXML** veya başka bir mod yönetsin istiyorsanız **[o_o]** KAPALI bırakın."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.WorkersScalar)), "Azami çalışan sayısı" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.WorkersScalar)),
                    "**İzin verilen azami çalışan sayısını artırır**.\n" +
                    "**100%** = oyunun varsayılan değeri."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ResetGameDefaults)), "Kaydırıcıları sıfırla" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ResetGameDefaults)),
                    "Yüzde kaydırıcılarını **100%**, cenaze aracı uyarı gecikmesini **3 dakika** yapar." },

                // STATUS fields (SHORT labels; left column is narrow!)

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary1)), "Cenaze aracı gerekli" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary1)),
                    "**Cenaze aracının almasını bekleyen ölü vatandaşlar.**"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary2)), "Miktar" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary2)),
                     "Oyun istatistiklerinden **aylık toplamlar**.\n" +
                     "**Maks. kapasite/ay** = mevcut verimlilikte krematoryum işleme ile mezar yeri yenilenmesinin toplamı.\n" +
                     "Bu, tüm aktif cenaze hizmeti tesislerinin ayda işleyebileceği azami ceset sayısıdır."
                 },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary3)), "Varlıklar" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary3)),
                    "**Aktif bina kapasiteleri:** toplam cenaze aracı, binalar, azami çalışanlar.\n\n" +
                    "**Notlar:**\n" +
                    "▪ Cenaze aracı: Aktif-parkta değil / (Toplam* cenaze aracı)\n" +
                    "▪ *Toplam cenaze aracı:\n" +
                    "== bakımdaki cenaze araçlarını içerir (ör. düşük hizmet bütçesi), \n" +
                    "== devre dışı binalardaki cenaze araçlarını içermez.\n" +
                    "▪ Durum taraması yalnızca Seçenekler açıkken (veya bir kaydırıcı kullandığınızda) çalışır; " +
                    "şehirde her karede çalışmadığından performansa neredeyse hiç etkisi yoktur :)"
                },

                // Status text templates
                { "MH_STATUS_NOT_LOADED", "Durum yüklenmedi." },
                { "MH_STATUS_NO_CITY_LOADED", "Şehir yüklenmedi." },
                { "MH_STATUS_STATS_NOT_AVAIL", "Şehir yok... ¯\\_(ツ)_/¯ ...İstatistik yok" },

                { "MH_STATUS_LINE1", "{0} bekliyor | {1} ölüm/ay | güncellendi {2}" },
                { "MH_STATUS_LINE2", "{0} maks. kapasite/ay | {1}/{2} mezar kullanılıyor" },
                { "MH_STATUS_LINE3", "{0} / {1} cenaze aracı | {2} / {3} bina | {4} azami çalışan" },
                { "MH_STATUS_PROCESSING_SUGGESTED", "Şu an önerilen: ~{0}% krematoryum işleme" },
                { "MH_STATUS_PROCESSING_MORE", "Şu an önerilen: 500% krematoryum işleme + daha fazla aktif tesis" },
                { "MH_STATUS_PROCESSING_NONE", "Öneri: krematoryumları açın/ekleyin" },

                // Cemetery reset tally (session status; row + named list below Assets)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary4)), "Mezarlık" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary4)),
                    "**Bu oturumda boşaltılan dolu mezarlıklar**.\n" +
                    "Toplam sıfırlama sayısını ve kaç farklı mezarlık olduğunu gösterir.\n" +
                    "Yeniden başlatınca veya şehir değiştirdiğinizde Durum temizlenir."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusCemetery1)), "▪" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusCemetery1)),
                    "Hangi mezarlıkların boşaltıldığını ve her birinin kaç kez boşaltıldığını gösterir (ad × sayı)." },

                { "MH_STATUS_LINE4", "sıfırlama: {0} · mezarlık: {1}" },
                { "MH_STATUS_CEMETERY_NONE", "bu oturumda yok" },
                { "MH_STATUS_CEMETERY_ROW", "{0} ×{1}" },
                { "MH_STATUS_CEMETERY_MORE", "+{0} daha" },

                // About
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AboutName)), "Mod" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AboutName)), "Bu modun görünen adı." },
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AboutVersion)), "Sürüm" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AboutVersion)), "Geçerli sürüm." },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.OpenParadoxMods)), "Paradox Mods" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.OpenParadoxMods)),
                    "Yazarın Paradox modları sayfasını açar." },

                // Debug report
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.LogReport)), "Günlük Raporu" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.LogReport)),
                    "MagicHearse.log dosyasına ayrıntılı bir ölüm hizmetleri raporu ve olası sorun alanlarını yazar." },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.OpenLog)), "Günlüğü Aç" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.OpenLog)),
                    "Varsa **Logs/MagicHearse.log** dosyasını açar.\n" +
                    "Dosya henüz bulunamazsa bunun yerine Logs klasörünü açar." },
            };
        }

        public void Unload()
        { }
    }
}
