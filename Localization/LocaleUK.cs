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
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAdvancedGrp), "Розширені" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kStatusGrp), "Стан" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAboutInfoGrp), "Інформація про мод" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kAboutLinksGrp), "Посилання" },
                { m_Setting.GetOptionGroupLocaleID(MHSetting.kDebugGrp), "Налагодження" },

                // Auto Clean (magic)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.EnableMagicHearse)), "Увімкнути магічне очищення" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.EnableMagicHearse)),
                    "Автоматично прибирає тіла померлих, яким потрібне перевезення катафалком.\n" +
                    "Магічне очищення та самостійне керування взаємовиключні; виберіть одне з них.\n" +
                    "Вимкніть усі прапорці, щоб деактивувати мод без його видалення.\n" +
                    "Технічна примітка: мають бути IsDead = true та WaitingForHearse = true."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.MagicResetCemetery)), "Скинути заповнене кладовище" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.MagicResetCemetery)),
                    "**Спорожнює заповнене кладовище**, щоб його не блокувала піктограма ЗАПОВНЕНО.\n" +
                    "Магічне очищення прибирає більшість тіл до поховання — цей параметр також очищає будь-яке кладовище, яке **вже заповнене**.\n" +
                    "<[ ] Типово ВИМКНЕНО>.\n" +
                    "Увімкніть цей параметр лише якщо режим магічного очищення також має спорожнювати вже заповнені кладовища.\n" +
                    "Після очищення зазвичай немає потреби залишати цей параметр увімкненим, доки магічне очищення залишається активним."
                },

                // Self Manage (FD)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.FuneralDirector)), "Керівник похоронної служби" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.FuneralDirector)),
                    "Самостійно керуйте та оптимізуйте звичайні системи похоронних служб гри.\n" +
                    "**Масштабні значення:** швидкість, парк, місткість.\n" +
                    "Додатково: **збільшити також кількість працівників**."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ProcScalar)), "Обробка крематорію" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ProcScalar)),
                    "**Швидкість обробки крематорію.**\n" +
                    "Вищі значення швидше кремують тіла та раніше звільняють місце у закладі.\n" +
                    "**100%** = стандартне значення гри."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.FleetScalar)), "Усього катафалків" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.FleetScalar)),
                    "**Максимальна кількість катафалків** на заклад.\n" +
                    "**100%** = стандартне значення гри.\n" +
                    "**[Примітка]** Надто багато катафалків може впливати на рух залежно від рівня смертності."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.HearseSpeedScalar)), "Швидкість катафалка" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.HearseSpeedScalar)),
                    "**Збільшує максимально дозволену швидкість руху катафалка**.\n" +
                    "**100%** = стандартне значення гри.\n" +
                    "<Обмеження швидкості на дорогах усе одно діють>.\n" +
                    "\n" +
                    "Також м’яко масштабує прискорення/гальмування, щоб нова максимальна швидкість не спричиняла надто різких стартів або зупинок.\n" +
                    "Примітка: навіть якщо максимальну швидкість катафалка збільшено, фактична швидкість залежить від:\n" +
                    "максимуму для транспортного засобу, обмеження дороги, безпечної швидкості ШІ гри (повороти, пошкодження дороги) та руху."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.HearseWarningMinutes)), "Затримка сповіщення про смерть (хв)" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.HearseWarningMinutes)),
                    "Це загальний час, за який катафалк має дістатися будівлі, перш ніж з’являться піктограми проблеми **очікування катафалка**.\n" +
                    "**3 хвилини** близькі до стандартного значення гри — приблизно 2,5 хвилини симуляції.\n" +
                    "Це значення можна збільшити, щоб дати катафалкам розумніший час завершити поїздку до появи піктограми смерті.\n" +
                    "Примітка:\n" +
                    "- <Рекомендовано: 10 хвилин>. Для міст із сильними заторами спробуйте більше.\n" +
                    "- У звіті Стан унизу можна перевірити, скільки випадків прострочено.\n" +
                    "- Уже видимі піктограми не приховуються, коли це значення вперше збільшується; вони залишаються, доки проблему не вирішить катафалк або будівлю не буде знесено.\n" +
                    "- Дозвольте поточним виїздам завершитися природно або один раз використайте прапорець <Магічне очищення [x]>, щоб швидко почати заново з новими часовими налаштуваннями."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StorageScalar)), "Місткість кладовища" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StorageScalar)),
                    "**Місткість кладовища** для головної будівлі.\n" +
                    "Більша місткість дозволяє заповненому кладовищу знову приймати тіла.\n" +
                    "Це не відправляє більше катафалків, якщо лише нестача місця не блокувала заклад.\n" +
                    "**100%** = стандартне значення гри."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AutoResetCemetery)), "Автоскидання кладовища" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AutoResetCemetery)),
                    "**Спорожнює заповнене кладовище**, щоб його не блокувала піктограма ЗАПОВНЕНО над будівлею.\n" +
                    "Більше не потрібно видаляти й перебудовувати заповнені кладовища.\n" +
                    "Вимкніть це, щоб натомість використовувати поступову **швидкість обороту кладовища**.\n" +
                    "<[ ✓ ] Типово УВІМКНЕНО>"
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.CemeteryTurnoverScalar)), "Швидкість обороту кладовища" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.CemeteryTurnoverScalar)),
                    "**Поступово звільняє зайняті могили кладовища.**\n" +
                    "Вищі значення роблять місця знову доступними швидше, ніж у базовій грі.\n" +
                    "Якщо кладовища все одно надто часто заповнюються при 500%,\n" +
                    "увімкніть натомість **[Автоскидання кладовища]**.\n" +
                    "**100%** = стандартна швидкість гри для повторного використання могил."
                },

                // Workers compatibility toggle
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ControlWorkers)), "Налаштувати працівників" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ControlWorkers)),
                    "Перемикач сумісності:\n" +
                    "**Увімкніть [✓]**, щоб збільшити кількість працівників.\n" +
                    "**[o_o]** Залиште ВИМКНЕНО, якщо хочете, щоб **ConfigXML** або інший мод керував працівниками похоронних служб."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.WorkersScalar)), "Максимум працівників" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.WorkersScalar)),
                    "**Збільшує максимальну кількість працівників**.\n" +
                    "**100%** = стандартне значення гри."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.ResetGameDefaults)), "Скинути повзунки" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.ResetGameDefaults)), "Встановлює відсоткові повзунки на **100%**, а затримку сповіщення про смерть — на **3 хвилини**." },

                // STATUS fields (SHORT labels; left column is narrow!)

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary1)), "Потрібен катафалк" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary1)),
                    "**Очікують** = усі померлі громадяни, які ще перебувають назовні й очікують на вивезення.\n" +
                    "**Прострочено** = громадяни в очікуванні, для яких закінчилася вибрана затримка сповіщення.\n" +
                    " - Якщо прострочених випадків багато, збільште час у параметрі Затримка сповіщення про смерть."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary2)), "Обсяг" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary2)),
                    "**Місячні підсумки** зі статистики гри.\n" +
                    "**Макс./міс.** = обробка крематоріїв плюс оборот кладовищ за поточної ефективності.\n" +
                    "Це максимальна кількість тіл, яку всі активні похоронні заклади могли б обробити за місяць."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary3)), "Ресурси" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary3)),
                    "**Місткість активних будівель:** усього катафалків, будівлі, максимум працівників.\n" +
                    "\n" +
                    "**Примітки:**\n" +
                    "▪ Катафалк: Активний-не припаркований / (Усього* катафалків)\n" +
                    "▪ *Усього катафалків:\n" +
                    "== включає катафалки на обслуговуванні (напр., при низькому бюджеті служби), \n" +
                    "== не включає катафалки вимкнених будівель.\n" +
                    "▪ Сканування стану працює лише коли відкрито Параметри (або використовується повзунок); воно не виконується щокадрово в місті, тож практично не впливає на продуктивність :)"
                },

                // Status text templates
                { "MH_STATUS_NOT_LOADED", "Стан не завантажено." },
                { "MH_STATUS_NO_CITY_LOADED", "Місто не завантажено." },
                { "MH_STATUS_STATS_NOT_AVAIL", "Немає міста... ¯\\_(ツ)_/¯ ...Немає статистики" },

                { "MH_STATUS_LINE1_V2", "{0} очікують | {1} прострочено | {2} смертей/міс." },
                { "MH_STATUS_LINE2_V2", "{0} макс./міс." },
                { "MH_STATUS_LINE3", "{0} / {1} катафалків | {2} / {3} будівель | {4} макс. працівників" },
                { "MH_STATUS_UPDATED", "оновлено {0}" },
                { "MH_STATUS_PROCESSING_SUGGESTED", "зараз рекомендовано: ~{0}% обробки крематорію" },
                { "MH_STATUS_PROCESSING_MORE", "зараз рекомендовано: 500% обробки крематорію + більше активних закладів" },
                { "MH_STATUS_PROCESSING_NONE", "рекомендовано: увімкніть/додайте крематорії" },

                // Cemetery reset tally (session status; row + named list below Assets)
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.StatusSummary4)), "Кладовище" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.StatusSummary4)),
                    "Показує **використані могили**, активні кладовища та скидання заповнених кладовищ у цій сесії.\n" +
                    "Стан очищується після перезапуску або зміни міста."
                },

                { "MH_STATUS_LINE4_V2", "{0} / {1} могил використано | {2} заклади | {3}" },
                { "MH_STATUS_RESET_SINGULAR", "{0} скидання" },
                { "MH_STATUS_RESET_PLURAL", "{0} скидань" },
                { "MH_STATUS_CEMETERY_NONE", "немає в цій сесії" },
                { "MH_STATUS_CEMETERY_ROW", "{0} ×{1}" },
                { "MH_STATUS_CEMETERY_MORE", "+ще {0}" },

                // About
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AboutName)), "Мод" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AboutName)), "Відображувана назва цього мода." },
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.AboutVersion)), "Версія" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.AboutVersion)), "Поточна версія." },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.OpenParadoxMods)), "Paradox Mods" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.OpenParadoxMods)), "Відкриває сторінку автора в Paradox Mods." },

                // Debug report
                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.LogReport)), "Звіт журналу" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.LogReport)), "Записує докладний звіт про похоронні служби та ймовірні проблемні місця до MagicHearse.log." },

                { m_Setting.GetOptionLabelLocaleID(nameof(MHSetting.OpenLog)), "Відкрити журнал" },
                { m_Setting.GetOptionDescLocaleID(nameof(MHSetting.OpenLog)),
                    "Відкриває **Logs/MagicHearse.log**, якщо файл існує.\n" +
                    "Якщо файлу ще немає, натомість відкриває папку Logs."
                },
            };
        }

        public void Unload()
        { }
    }
}
