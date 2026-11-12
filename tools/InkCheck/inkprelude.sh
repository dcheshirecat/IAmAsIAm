#!/bin/bash
# Usage: inkprelude.sh N -> compiles & auto-plays draft_prelude_partN alone, then restores main.ink.
cd /home/user/IAmAsIAm; S=Assets/_Project/Story; cp $S/main.ink /tmp/claude-0/main.bak
sed -i "s/^INCLUDE chapters_placeholder.ink/INCLUDE chapters_placeholder.ink\nINCLUDE Drafts\/draft_prelude_part$1.ink/; s/^-> start$/-> prelude_part$1/" $S/main.ink
bash tools/InkCheck/check.sh 2>&1 | grep -E "✓|✗|rror|tag '|Warn" | head -12
cp /tmp/claude-0/main.bak $S/main.ink
