.equ universalSmallKeys, multiplierTable+4
.thumb
cmp	r7,#0xFF
bhi	vanilla
cmp	r7,#0
beq	vanilla
mov	r1,r7
sub	r1,#0x17

cmp	r4,#1
bne	new
ldr	r0,multiplierTable
ldrb	r0,[r0,r1]
mov	r4,r0
b	new

vanilla:
ldr	r0,=#0x2033A90
ldrb	r1,[r0,#3]

new:
ldr	r0,universalSmallKeys
cmp	r0,#0
beq	notUniversal
mov	r1,#0

notUniversal:
ldr	r0,=#0x2002E9C
add	r1,r0
ldrb	r0,[r1]
add	r0,r4
ldr	r3,=#0x805232D
bx	r3

.align
.ltorg
multiplierTable:
@POIN multiplierTable
@WORD universalSmallKeys
