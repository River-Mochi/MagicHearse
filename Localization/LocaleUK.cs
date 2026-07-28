// <copyright file="LocaleUK.cs" company="River-Mochi">
// Copyright (c) 2026 River-Mochi. All rights reserved.
// Licensed under the MIT License. You may not use this file except in compliance with this License.
// See LICENSE file in the project root for full license information.
// This notice and the MIT License notice must be kept with
// all copies or substantial portions of this code.
// ================= </copyright> ======================

// File: Localization/LocaleUK.cs
// Ukrainian uk-UA locale for Magic Hearse.

namespace MagicHearse
{
    using System.Collections.Generic; // IEnumerable, Dictionary, KeyValuePair

    using Colossal; // IDictionarySource, IDictionaryEntryError

    /// <summary>
    /// Ukrainian localization source for Magic Hearse [MH].</summary>
    public sealed class LocaleUK : IDictionarySource
    {
        private readonly MHSetting m_Setting;

        /// <summary>
        /// Constructs the Ukrainian locale generator.</summary>
        /// <param name="setting">Settings object used for locale IDs.</param>
        public LocaleUK(MHSetting setting)
        {
            m_Setting = setting;
        }

        /// <summary>
        /// Creates all Ukrainian localization entries for this mod.</summary>
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
                { m_Setting.GetOptionTabLocaleID(MHSetting.kActionsTab), "Дії" },
                { m_Setting.GetOptionTabLocaleID(MHSetting.kAboutTab), "Про мод" },

                // Groups
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAutoCleanGrp), "Автоочищення" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kSelfManageGrp), "Самостійне керування" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAdvancedGrp), "Додатково" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kStatusGrp), "Стан" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAboutInfoGrp), "Інформація про мод" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAboutLinksGrp), "Посилання" },

                // Auto Clean (magic)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.EnableMagicHearse)), "Увімкнути магічне очищення" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.EnableMagicHearse)),
                    "Автоматично видаляє тіла, які потребують перевезення (катафалком).\n" +
                    "Магічне очищення та самостійне керування взаємовиключні; виберіть один із цих режимів.\n" +
                    "Вимкніть усі прапорці, щоб вимкнути мод, не видаляючи його.\n" +
                    "Технічна примітка: потрібні IsDead = true та WaitingForHearse = true."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.MagicResetCemetery)), "Скинути заповнене кладовище" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.MagicResetCemetery)),
                    "**Очищає будь-яке заповнене кладовище**, щоб його не блокувала піктограма «ЗАПОВНЕНО».\n" +
                    "Магічне очищення видаляє більшість тіл до поховання — ця опція все одно очищає будь-яке кладовище, яке **вже заповнене**.\n" +
                    "<[ ] За замовчуванням ВИМКНЕНО>.\n" +
                    "Увімкніть цю опцію, лише якщо режим магічного очищення також має очищати вже заповнені кладовища.\n" +
                    "Після очищення зазвичай немає потреби залишати цю опцію ввімкненою, доки магічне очищення залишається ввімкненим."
                },

                // Self Manage (FD)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.FuneralDirector)), "Керівник ритуальної служби" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.FuneralDirector)),
                    "Керуйте всім самостійно.\n" +
                    "**Масштабуйте значення:** швидкість, автопарк, місткість.\n" +
                    "Додатково можна **збільшити кількість працівників**."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ProcScalar)), "Швидкість обробки" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ProcScalar)),
                    "**Швидкість роботи закладу** (кремації)\n" +
                    "**100%** = стандартне значення гри."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.FleetScalar)), "Розмір автопарку" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.FleetScalar)),
                    "**Максимальна кількість катафалків** на заклад.\n" +
                    "**100%** = стандартне значення гри.\n" +
                    "**[Примітка]** Забагато катафалків може вплинути на рух залежно від рівня смертності."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StorageScalar)), "Місткість кладовища" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StorageScalar)),
                    "**Місткість головної будівлі кладовища**.\n" +
                    "**100%** = стандартне значення гри."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AutoResetCemetery)), "Скинути заповнене кладовище" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AutoResetCemetery)),
                    "**Очищає кладовище**, коли воно заповнене, щоб його не блокувала піктограма «ЗАПОВНЕНО» над будівлею.\n" +
                    "Більше не потрібно видаляти й перебудовувати заповнені кладовища.\n" +
                    "Працює разом із повзунком **Місткість кладовища**: налаштуйте розмір кладовищ і дозвольте їм повторно використовуватися, щоб більше ніколи не зносити заповнене кладовище.\n" +
                    "<[ ✓ ] УВІМКНЕНО за замовчуванням>"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.HearseSpeedScalar)), "Швидкість катафалка" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.HearseSpeedScalar)),
                    "**Збільшує максимальну швидкість катафалка**.\n" +
                    "**100%** = стандартне значення гри.\n" +
                    "<Обмеження швидкості на дорогах усе ще діють>.\n\n" +
                    "Також плавно масштабує прискорення й гальмування, щоб нова максимальна швидкість не спричиняла різких стартів і зупинок.\n" +
                    "Примітка: навіть зі збільшеною максимальною швидкістю катафалка на фактичну швидкість руху впливають:\n" +
                    "дозволений максимум авто, обмеження дороги, безпечна швидкість ігрового ШІ (повороти, пошкодження доріг) і затори."
                },

                // Workers compatibility toggle
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ControlWorkers)), "Керувати макс. працівниками" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ControlWorkers)),
                    "Перемикач сумісності:\n" +
                    "**Увімкніть [✓]**, щоб збільшити кількість працівників.\n" +
                    "**[o_o]** Залиште ВИМКНЕНО, якщо працівниками ритуальних служб має керувати **ConfigXML** або інший мод."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.WorkersScalar)), "Макс. працівників" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.WorkersScalar)),
                    "**Збільшує дозволену максимальну кількість працівників**.\n" +
                    "**100%** = стандартне значення гри."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ResetGameDefaults)), "Скинути повзунки" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ResetGameDefaults)), "Повертає всі повзунки до **100%** (стандартні значення гри)." },

                // STATUS fields (SHORT labels; left column is narrow!)

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary1)), "Потрібен катафалк" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary1)),
                    "**Померлі містяни чекають** на прибуття катафалка."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary2)), "Обсяг" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary2)),
                    "**Місячні підсумки** зі статистики гри.\n" +
                    "**Макс. кремацій/міс.** = показник Обробка/міс. на інформаційній панелі гри.\n" +
                    "Це максимальна кількість тіл, яку крематорії можуть обробити за місяць."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary3)), "Ресурси" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary3)),
                    "**Активні можливості будівель:** загальна кількість катафалків, будівель і макс. працівників.\n\n" +
                    "**Примітки:**\n" +
                    "▪ Катафалки: активні, не припарковані / (усього* катафалків)\n" +
                    "▪ *Усього катафалків:\n" +
                    "== включає катафалки на обслуговуванні (наприклад, через низький бюджет служби), \n" +
                    "== не включає катафалки вимкнених будівель.\n" +
                    "▪ Сканування стану виконується лише тоді, коли відкрито меню параметрів (або використано повзунок); " +
                    "воно не працює щокадрово в місті, тому практично не впливає на продуктивність :)"
                },

                // Status text templates
                { "MH_STATUS_NOT_LOADED", "Стан не завантажено." },
                { "MH_STATUS_NO_CITY_LOADED", "Місто не завантажено." },
                { "MH_STATUS_STATS_NOT_AVAIL", "Немає міста... ¯\\_(ツ)_/¯ ...Немає статистики" },

                { "MH_STATUS_LINE1", "{0} очікують | {1} смертей/міс. | оновлено {2}" },
                { "MH_STATUS_LINE2", "{0} макс. кремацій/міс. | {1}/{2} могил зайнято" },
                { "MH_STATUS_LINE3", "{0} / {1} катафалків | {2} / {3} будівель | {4} макс. працівників" },

                // Cemetery reset tally (session status; row + named list below Assets)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary4)), "Кладовища" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary4)),
                    "**Кладовища, автоматично очищені в цій сесії** функцією «Скинути заповнене кладовище».\n" +
                    "Показує загальну кількість скидань і кількість різних кладовищ.\n" +
                    "Очищується після перезапуску або зміни міста."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusCemetery1)), "▪" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusCemetery1)), "Які кладовища було очищено та скільки разів кожне (назва × кількість)." },

                { "MH_STATUS_LINE4", "скидань: {0} · кладовищ: {1}" },
                { "MH_STATUS_CEMETERY_NONE", "немає в цій сесії" },
                { "MH_STATUS_CEMETERY_ROW", "{0} ×{1}" },
                { "MH_STATUS_CEMETERY_MORE", "+ще {0}" },

                // About
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AboutName)), "Мод" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AboutName)), "Назва мода, що відображається." },
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AboutVersion)), "Версія" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AboutVersion)), "Поточна версія." },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.OpenParadoxMods)), "Paradox Mods" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.OpenParadoxMods)), "Відкриває сторінку модів автора на Paradox Mods." },
            };
        }

        public void Unload()
        { }
    }
}
