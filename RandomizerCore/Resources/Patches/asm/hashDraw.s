.equ versionNumber, hashIconsTable+4
.thumb
ldr	r3,=#0x801D668
mov	lr,r3
.short	0xF800
ldr	r1,=#0x3000F50
mov	r0,#1
strh	r0,[r1,#0x0E]

@check if file select
push	{r0-r7}
ldr	r0,=#0x3001002
ldrb	r0,[r0]
cmp	r0,#1
bne	end
@draw the tiles
ldr	r4,=#0x811DBD5
mov	r5,#0
ldr	r6,=#0x2034CB6
ldr	r7,hashIconsTable
iconsLoop:
ldrb	r0,[r4,r5]
mov	r1,#0x3F
and	r0,r1
lsl	r0,#1
ldrh	r0,[r7,r0]
lsl	r1,r5,#2
orr	r0,r1
add	r0,#0x04
strh	r0,[r6]
add	r0,#1
strh	r0,[r6,#2]
add	r0,#1
add	r6,#0x40
strh	r0,[r6]
add	r0,#1
strh	r0,[r6,#2]
sub	r6,#0x40
add	r6,#4
add	r5,#1
cmp	r5,#5
bne	np
ldr	r0,=#0x0C5F
mov	r1,#0x40
strh	r0,[r6,r1]
add	r6,#2
np:
cmp	r5,#7
blo	iconsLoop


@draw version number as well:
ldr	r0, versionNumber
cmp	r0, #0
beq	end
ldr	r3,=#0x2034CB0 + (0x01*0x20*2) + (0x1B*2)
mov	r1, #'.'
lsl	r2, r0, #24
lsr	r2, #24
lsr	r0, #8
cmp	r2, #0xFF
beq	noLetter
add	r2, #'A'
strh	r2, [r3]
sub	r3, #2
noLetter:
lsl	r2, r0, #24
lsr	r2, #24
lsr	r0, #8
add	r2, #'0'
strh	r2, [r3]
sub	r3, #2
strh	r1, [r3]
sub	r3, #2
lsl	r2, r0, #24
lsr	r2, #24
lsr	r0, #8
add	r2, #'0'
strh	r2, [r3]
sub	r3, #2
strh	r1, [r3]
sub	r3, #2
add	r0, #'0'
strh	r0, [r3]
sub	r3, #2

end:
pop	{r0-r7}
pop	{pc}
.align
.ltorg
hashIconsTable:
@POIN hashIconsTable
@POIN versionNumber
