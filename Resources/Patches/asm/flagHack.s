.thumb
push	{r4,lr}
mov	r4,r1
ldr	r0,[r4]

@check if there is a match with our changed flags
ldr	r1,flags
loop:
ldr	r2,[r1]
cmp	r2,#0
beq	Vanilla
cmp	r2,r0
beq	New
add	r1,#12
b	loop

Vanilla:
ldr	r3,=#0x80169CE
mov	lr,r3
.short	0xF800
ldr	r3,=#0x807DBB6
mov	lr,r3
.short	0xF800

New:
@check the new flag
ldr	r2,[r1,#4]
ldr	r3,[r1,#8]
ldrb	r0,[r2]
and	r0,r3
cmp	r0,r3
bne	EndNewFalse
mov	r0,#1
b	EndNewTrue
EndNewFalse:
mov	r0,#0
EndNewTrue:
ldr	r3,=#0x807DBE8
mov	lr,r3
.short	0xF800

.align
.ltorg
flags:
