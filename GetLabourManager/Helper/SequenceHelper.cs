using System.Linq;
using GetLabourManager.Models;


namespace GetLabourManager.Helper
{
    public class SequenceHelper
    {
        public enum NType
        {
            EMPLOYEE,
            TRANSACTION,
            PAYROLL,
            GANG_ADVICE,
            GANG_NUMBER,
            COSTSHEET,
            INVOICING,
            TIMEAUDIT
        }

//        private static readonly RBACDbContext db = new RBACDbContext();

        public static string getSequence(RBACDbContext db,NType t)
        {
            var sequence = "";
            switch (t)
            {
                case NType.EMPLOYEE:
                    var entity = db.Sequence.FirstOrDefault(x => x.SequenceType == "EMPLOYEE");
                    var number = (entity.SequenceNumber) + 1;
                    var length = entity.SequenceLength;
                    // ReSharper disable once ConvertConditionalTernaryToNullCoalescing
                    var prefix = entity.SequencePrefix ?? "";
                    var suffix = entity.SequenceSuffix ?? "";
                    var build = "";
                    length -= number.ToString().Length;
                    for (var i = 0; i < length; i++)
                    {
                        build += "0";
                    }
                    sequence = prefix + "" + build + "" + number + "" + suffix;
                    break;
                case NType.TRANSACTION:
                    var entitysubject = db.Sequence.FirstOrDefault(x => x.SequenceType == "TRANSACTION");
                    var numbers = (entitysubject.SequenceNumber) + 1;
                    var lengths = (entitysubject.SequenceLength);
                    var prefixs = entitysubject.SequencePrefix != null ? entitysubject.SequencePrefix : "";
                    var suffixs = entitysubject.SequenceSuffix != null ? entitysubject.SequenceSuffix : "";
                    var builds = "";
                    lengths -= numbers.ToString().Length;

                    if (lengths > 0)
                    {
                        for (var i = 0; i < lengths; i++)
                        {
                            builds += "0";
                        }
                        sequence = prefixs + "" + builds + "" + numbers + "" + suffixs;
                    }
                    else
                    {
                        int number_in = (entitysubject.SequenceLength);
                        int off_mark = (lengths * -1)+number_in;
                        int length_off_set = off_mark.ToString().Length;
                        int _raised_length = number_in - length_off_set;
                        for (var i = 0; i < _raised_length; i++)
                        {
                            builds += "0";
                        }
                        sequence = prefixs + "" + builds + "" + off_mark + "" + suffixs;
                    }
                    break;
                case NType.COSTSHEET:
                    var entityP = db.Sequence.FirstOrDefault(x => x.SequenceType == "COST SHEET");
                    var numberP = (entityP.SequenceNumber) + 1;
                    var lengthP = (entityP.SequenceLength);
                    var prefixP = entityP.SequencePrefix ?? "";
                    var suffixP = entityP.SequenceSuffix ?? "";
                    var buildP = "";
                    lengthP -= numberP.ToString().Length;
                    for (var i = 0; i < lengthP; i++)
                    {
                        buildP += "0";
                    }
                    sequence = prefixP + "" + buildP + "" + numberP + "" + suffixP;
                    break;
                case NType.GANG_ADVICE:
                    var entity_ga = db.Sequence.FirstOrDefault(x => x.SequenceType == "GANG ADVICE");
                    var number_ga = (entity_ga.SequenceNumber) + 1;
                    var length_ga = (entity_ga.SequenceLength);
                    var prefix_ga = entity_ga.SequencePrefix ?? "";
                    var suffix_ga = entity_ga.SequenceSuffix ?? "";
                    var build_ga = "";
                    length_ga -= number_ga.ToString().Length;
                    for (var i = 0; i < length_ga; i++)
                    {
                        build_ga += "0";
                    }
                    sequence = prefix_ga + "" + build_ga + "" + number_ga + "" + suffix_ga;
                    break;
                case NType.GANG_NUMBER:
                    var entity_NN = db.Sequence.FirstOrDefault(x => x.SequenceType == "GANG NUMBER");
                    var number_nn = (entity_NN.SequenceNumber) + 1;
                    var length_nn = (entity_NN.SequenceLength);
                    var prefix_nn = entity_NN.SequencePrefix ?? "";
                    var suffix_nn = entity_NN.SequenceSuffix ?? "";
                    var build_nn = "";
                    length_nn -= number_nn.ToString().Length;
                    for (var i = 0; i < length_nn; i++)
                    {
                        build_nn += "0";
                    }
                    sequence = prefix_nn + "" + build_nn + "" + number_nn + "" + suffix_nn;
                    break;
                case NType.INVOICING:
                    var entity_INN = db.Sequence.FirstOrDefault(x => x.SequenceType == "INVOICING");
                    var number_inn = (entity_INN.SequenceNumber) + 1;
                    var length_inn = (entity_INN.SequenceLength);
                    var prefix_inn = entity_INN.SequencePrefix ?? "";
                    var suffix_inn = entity_INN.SequenceSuffix ?? "";
                    var build_inn = "";
                    length_inn -= number_inn.ToString().Length;
                    for (var i = 0; i < length_inn; i++)
                    {
                        build_inn += "0";
                    }
                    sequence = prefix_inn + "" + build_inn + "" + number_inn + "" + suffix_inn;
                    break;
                case NType.TIMEAUDIT:
                    var entity_INNT = db.Sequence.FirstOrDefault(x => x.SequenceType == "TIME-AUDIT");
                    var number_innt = (entity_INNT.SequenceNumber) + 1;
                    var length_innt = (entity_INNT.SequenceLength);
                    var prefix_innt = entity_INNT.SequencePrefix ?? "";
                    var suffix_innt = entity_INNT.SequenceSuffix ?? "";
                    var build_innt = "";
                    length_innt -= number_innt.ToString().Length;
                    for (var i = 0; i < length_innt; i++)
                    {
                        build_innt += "0";
                    }
                    sequence = prefix_innt + "" + build_innt + "" + number_innt + "" + suffix_innt;
                    break;
            }
            return sequence;
        }

        public static void IncreaseSequence(RBACDbContext db,NType t)
        {
            switch (t)
            {
                case NType.EMPLOYEE:
                    var entity = db.Sequence.FirstOrDefault(x => x.SequenceType == "EMPLOYEE");
                    if (entity != null)
                    {
                        entity.SequenceNumber += 1;
                    }
                    db.SaveChanges();
                    break;
                case NType.TRANSACTION:
                    var entitytransaction = db.Sequence.FirstOrDefault(x => x.SequenceType == "TRANSACTION");
                    if (entitytransaction != null)
                    {
                        entitytransaction.SequenceNumber += 1;
                    }
                    db.SaveChanges();
                    break;
                case NType.COSTSHEET:
                    var entitysubject = db.Sequence.FirstOrDefault(x => x.SequenceType == "COST SHEET");
                    if (entitysubject != null)
                    {
                        entitysubject.SequenceNumber += 1;
                    }
                    db.SaveChanges();
                    break;
                case NType.GANG_ADVICE:
                    var entity_advice = db.Sequence.FirstOrDefault(x => x.SequenceType == "GANG ADVICE");
                    if (entity_advice != null)
                    {
                        entity_advice.SequenceNumber += 1;
                    }
                    db.SaveChanges();
                    break;
                case NType.GANG_NUMBER:
                    var entity_number = db.Sequence.FirstOrDefault(x => x.SequenceType == "GANG NUMBER");
                    if (entity_number != null)
                    {
                        entity_number.SequenceNumber += 1;
                    }
                    db.SaveChanges();
                    break;
                case NType.INVOICING:
                    var entity_number_invoice = db.Sequence.FirstOrDefault(x => x.SequenceType == "INVOICING");
                    if (entity_number_invoice != null)
                    {
                        entity_number_invoice.SequenceNumber += 1;
                    }
                    db.SaveChanges();
                    break;
                case NType.TIMEAUDIT:
                    var entity_number_invoicet = db.Sequence.FirstOrDefault(x => x.SequenceType == "TIME-AUDIT");
                    if (entity_number_invoicet != null)
                    {
                        entity_number_invoicet.SequenceNumber += 1;
                    }
                    db.SaveChanges();
                    break;
            }
        }
    }
}