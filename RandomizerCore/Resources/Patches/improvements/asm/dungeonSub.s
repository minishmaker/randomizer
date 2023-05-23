.equ universalCompasses, universalMaps+4
.equ universalBigKeys, universalCompasses+4
.thumb
cmp	r7,#0
beq	vanilla
mov	r0,r7
sub	r0,#0x17
b	new

vanilla:
ldrb	r0,[r0,#3]

new:
ldrb	r2,[r4,#2]
cmp	r2,#1
bne	notMap

ldr	r3,universalMaps
cmp	r3,#0
beq	notUniversal
mov	r0,#0
b	universal

notMap:
cmp	r2,#2
bne	notCompass

ldr	r3,universalCompasses
cmp	r3,#0
beq	notUniversal
mov	r0,#0
b	universal

notCompass:
ldr	r3,universalBigKeys
cmp	r3,#0
beq	notUniversal
mov	r0,#0

notUniversal:
add	r1,r0

universal:
ldrb	r0,[r1]
orr	r0,r2
ldr	r3,=#0x8053E13
bx	r3

.align
.ltorg
universalMaps:
@WORD universalMap
@WORD universalCompasses
@WORD universalBigKeys
