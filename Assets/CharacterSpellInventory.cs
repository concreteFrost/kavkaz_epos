using System.Collections.Generic;
using UnityEngine;

public class CharacterSpellInventory : MonoBehaviour
{
    public List<SpellData> spells = new List<SpellData>();

    private int _spellIndex;
    public int SpellIndex
    {
        get => _spellIndex;
        private set
        {
            if (spells.Count == 0)
            {
                _spellIndex = 0;
                return;
            }

            _spellIndex = (value % spells.Count + spells.Count) % spells.Count;
        }
    }

    public SpellData CurrentSpell =>
        spells.Count == 0 ? null : spells[SpellIndex];

    public void AddSpell(SpellData spellData)
    {
        if (spellData == null) return;

        spells.Add(spellData);

        // если это первый добавленный спелл
        if (spells.Count == 1)
            SpellIndex = 0;
    }

    public void ChangeSpell(int direction)
    {
        if (spells.Count == 0) return;

        SpellIndex += direction;
    }
    public void UseSpell()
    {
        
        CurrentSpell.quantity--;

        if (CurrentSpell.quantity <= 0)
        {
            spells.RemoveAt(SpellIndex);

            if (spells.Count == 0)
            {
                SpellIndex = 0;
                return;
            }

            // если удалили последний элемент,
            // индекс станет равен Count, корректируем
            if (SpellIndex >= spells.Count)
                SpellIndex = spells.Count - 1;
        }
    }
}
