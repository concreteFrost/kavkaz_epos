using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpellInventory : MonoBehaviour
{
    public List<ItemData> spells = new List<ItemData>();

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

    public Action<ItemData> UpdateSpell;

    public ItemData CurrentSpell =>
        spells.Count == 0 ? null : spells[SpellIndex];


    public void GetCurrentSpell()
    {
        UpdateSpell?.Invoke(CurrentSpell); 
    }

    public void AddSpell(ItemData spellData)
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

        UpdateSpell?.Invoke(CurrentSpell);
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
              
            }

            // если удалили последний элемент,
            // индекс станет равен Count, корректируем
            else if (SpellIndex >= spells.Count)
            {
                SpellIndex = spells.Count - 1;
                
            }
                
        }

        UpdateSpell?.Invoke(CurrentSpell);

    }
}
