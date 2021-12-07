using Dungeon;
using Dungeon12.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Dungeon12.Entities.Enums
{
    public enum Fraction
    {
        [Display(Name = "Авангард")]
        [Value("Основан как содружество различных военных организаций призваных сохранять мир на всём материке.")]
        [AvailableSpecs(Spec.WarriorDamage, Spec.WarriorWarchief, Spec.WarriorProtector)]
        [AvailableRoles(Roles.Damage,Roles.Trap,Roles.Tank)]
        Vanguard, // warrior

        [Display(Name = "Гильдия магов")]
        [Value("Главная организация Империи Ушал - сообщество магов занимающихся изучением элементальной магии.")]
        [AvailableSpecs(Spec.MageAoe, Spec.MageSorcerer, Spec.MagePriest)]
        [AvailableRoles(Roles.Magic, Roles.Heal, Roles.Range)]
        MageGuild, // elementalist

        [Display(Name = "Наёмники")]
        [Value("Организация официально не существующая, однако, имеющая большое влияние на весь материк.")]
        [AvailableSpecs(Spec.MercenaryLeader, Spec.MercenaryAssassin, Spec.MercenaryOutlaw)]
        [AvailableRoles(Roles.Tank, Roles.Range, Roles.Trap)]
        Mercenary, // rogues

        [Display(Name = "Экзархат")]
        [Value("Военизированное объединение всех служителей света населяющих материк.")]
        [AvailableSpecs(Spec.PaladinTemplar, Spec.PaladinAdept, Spec.InquisitorJudge, Spec.InquisitorTormentor)]
        [AvailableRoles(Roles.Vampyr, Roles.Heal, Roles.Magic)]
        Exarch, // paladins and priests

        [Display(Name = "Культ проклятых")]
        [Value("Образовался после катастрофы, состоит из магов-отшельников и служителей смерти.")]
        [AvailableSpecs(Spec.WarlockNecromancer, Spec.WarlockBloodMage)]
        [AvailableRoles(Roles.Vampyr, Roles.Debuff, Roles.Summon)]
        Cult // warlocks
    }
}